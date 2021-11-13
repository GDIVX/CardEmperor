using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance{get{return _instance;}}
    static WorldController _instance;


    public WorldGenData worldGenData;
    public Tilemap map;
    public WorldTile[,] world;



    //TODO if moving to multiplayer, this needs to go to player. Right now we leave it here for ease of access.
    public GameObject[,] overlays;
    public Stack<GameObject> disabledOverlays = new Stack<GameObject>();

    public GameObject CreatureTemplate;

    internal Vector3Int GetRandomTile()
    {
        int x = UnityEngine.Random.Range(0, world.GetLength(0));
        int y = UnityEngine.Random.Range(0, world.GetLength(1));

        return new Vector3Int(x,y,0);
    }

    private bool[,] territory;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            _instance = this;
        }

    }

    void Start()
    {
        GameManager.Instance.NotifyOnInitTask(false);
        WorldGenerator.GenerateWorld(worldGenData , map);
        territory = new bool[worldGenData.size.x , worldGenData.size.y];
        GameManager.Instance.NotifyOnInitTask(true);
        
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            OnMouseClick(true);
        }
        else if(Input.GetMouseButtonDown(1)){
            OnMouseClick(false);
        }
    }

    internal void AddTerritory(WorldTile tile)
    {

        Vector2Int position = tile.position;
        territory[position.x , position.y] = true;
        UpdateTerritory();
    }

    internal void AddWorkingTile(WorldTile tile)
    {
        int[] income = tile.GetIncome();
        Player.Main.foodPoints.income += income[0];
        Player.Main.industryPoints.income += income[1];
        Player.Main.magicPoints.income += income[2];
        Player.Main.knowledge.income += income[3];
    }
    private void UpdateTerritory()
    {
        for (var x = 0; x < territory.GetLength(0); x++)
        {
            for (var y = 0; y < territory.GetLength(1); y++)
            {
                if(territory[x,y]){
                    map.SetTile(new Vector3Int(x,y,0) , worldGenData.teritoryTile);
                }
            }
        }
    }

    public bool IsInTerritory(Vector3Int position){
        return territory[position.x , position.y];
    }
    void OnMouseClick(bool isLeftClick){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Vector3Int.RoundToInt(Input.mousePosition));
        Vector3Int gridPosition = map.WorldToCell(new Vector3(mousePosition.x , mousePosition.y , 0));

        if(map.HasTile(gridPosition)){
            WorldTile tile = world[gridPosition.x , gridPosition.y];
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
        if(
            position.x > 0 &&
            position.y > 0 && 
            position.x < world.GetLength(0) && 
            position.y < world.GetLength(1)){
                return true;
        }
        return false;
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
