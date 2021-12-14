using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Assets.Scripts.Mechanics.Systems.Players;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance{get{return _instance;}}
    static WorldController _instance;


    [TabGroup("Generation")]
    public WorldGenData worldGenData;
    [TabGroup("Generation")]
    public Tilemap map , indicatorsMap;
    public WorldTile[,] world;


    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Debug")]
    Vector3 _Debug_mousePosition;
    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Debug")]
    Vector3Int _Debug_gridPosition;


    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Debug")]
    WorldTile _Debug_sampledTile;
    [TabGroup("Debug")]
    public TileBase tileGizmo;
    public OverlayController overlayController;


    public GameObject CreatureTemplate;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            _instance = this;
        }
        
    }

    public void Init(){

        WorldGenerator.GenerateWorld(worldGenData , map);
        
        int x = Mathf.RoundToInt(worldGenData.size.x / 2);
        int y = Mathf.RoundToInt(worldGenData.size.y / 2);
        Creature capital = Creature.BuildAndSpawnCardless("Capital" , Player.Main.ID  , new Vector3Int(x,y,0));
        GameManager.Instance.capital = capital;
    }



    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            OnMouseClick(true);
        }
        else if(Input.GetMouseButtonDown(1)){
            OnMouseClick(false);
        }
    }
    internal Vector3Int GetRandomTile()
    {
        try{
            int x = UnityEngine.Random.Range(1, world.GetLength(0) - 1);
            int y = UnityEngine.Random.Range(1, world.GetLength(1) - 1);

            return new Vector3Int(x,y,0);
        }
        catch(NullReferenceException e){
            Debug.LogError($"Calling world before it was initialized : {e}");
            return Vector3Int.zero;
        }
    }
    internal Vector3Int GetRandomEmptyTile(int iterations = 40)
    {
        if(iterations <= 0){
            Debug.LogWarning("Can't find an empty tile");
            return new Vector3Int(-1,-1,-1);
        }
        Vector3Int pos = GetRandomTile();
        if(world[pos.x ,pos.y].CreatureID == 0 ||!world[pos.x ,pos.y].walkable){
            return pos;
        }
        return GetRandomEmptyTile(iterations-1);
    }




    internal void AddWorkingTile(WorldTile tile)
    {
        int[] income = tile.GetIncome();
        Player.Main.foodPoints.income += income[0];
        Player.Main.industryPoints.income += income[1];
        Player.Main.magicPoints.income += income[2];

        Player.Main.foodPoints.value += income[0];
        Player.Main.industryPoints.value += income[1];
        Player.Main.magicPoints.value += income[2];

    }
    void OnMouseClick(bool isLeftClick){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Vector3Int.RoundToInt(Input.mousePosition));
        Vector3Int gridPosition = map.WorldToCell(new Vector3(mousePosition.x , mousePosition.y , 0));

        _Debug_mousePosition = mousePosition;
        _Debug_gridPosition = gridPosition;

        if(map.HasTile(gridPosition)){
            WorldTile tile = world[gridPosition.x , gridPosition.y];
            _Debug_sampledTile = tile;
            if(isLeftClick){
                tile.OnLeftClick();
            }
            else{
                tile.OnRightClick();
            }
        }
    }

    public bool IsTileExist(Vector3Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < world.GetLength(0) && position.y < world.GetLength(1);
    }
    public static Vector3Int CordsToCube(Vector3Int cords){
        int q = cords.x - (cords.y - (cords.y&1)) / 2;
        int r = cords.y;
        int s = -q-r;
        Vector3Int cube = new Vector3Int(q,r,s);
        return cube;
    }

    public static Vector3Int CubeToCords(Vector3Int cube){
        int x = cube.x + (cube.y - (cube.y&1)) / 2;
        int y = cube.y;
        return new Vector3Int(x,y,0);
    }

    public static int DistanceOf(Vector3Int a , Vector3Int b){
        a = CordsToCube(a);
        b = CordsToCube(b);

        int x = Mathf.Abs(a.x - b.x);
        int y = Mathf.Abs(a.y - b.y);
        int z =Mathf.Abs(a.z - b.z);
        int distance = Mathf.Max(x,y,z);

        return distance;
    }

    public WorldTile[] GetLine(Vector3Int start , Vector3Int end , int maxLength = 999){
        List<WorldTile> res = new List<WorldTile>();
        int distance = DistanceOf(start , end);
        if( distance > maxLength){
            distance = maxLength;
        }

        for (var i = 0; i < distance; i++)
        {
            Vector3 lerp = Vector3.Lerp(start , end , 1f / distance * i);
            res.Add(world[Mathf.RoundToInt(lerp.x) , Mathf.RoundToInt(lerp.y)]);
        }

        return res.ToArray();
    }
}
