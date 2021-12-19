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
using Assets.Scripts.Mechanics.Components.Board.Pathfinding;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance{get{return _instance;}}
    static WorldController _instance;


    [TabGroup("Generation")]
    public WorldGenData worldGenData;
    [TabGroup("Generation")]
    public Tilemap map , indicatorsMap;
    public WorldTile[,] world;
    public Vector3Int mouseGridPosition => GetMouseGridPosition();


    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Debug")]
    Vector3 _Debug_mousePosition;



    [ShowInInspector]
    [ReadOnly]
    [TabGroup("Debug")]
    WorldTile _Debug_sampledTile;
    [TabGroup("Debug")]
    public TileBase tileGizmo;


    public OverlayController overlayController, tooltipOverlayController;


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

        if(Input.GetKeyDown(KeyCode.F11)){
            ShowPath();
        }
    }

    [Button]
    public void ShowPath(){

        CreatureDisplayer displayer = GameManager.CurrentSelected as CreatureDisplayer;
        if(displayer != null){
            ShowPathToMouse(displayer);
        }
    }

    private void ShowPathToMouse(CreatureDisplayer displayer)
    {
        
        WorldTile end = GetMouseTile();
        if(end == null) return;

        WorldTile start = GetTile(Creature.GetCreature(displayer.ID).position);

        Pathfinder pathfinder = new Pathfinder();
        var path = pathfinder.FindPath(start , end);

        if(path == null){
            Debug.Log("null path");
            return;
        }

        List<Vector3Int> pathVectors = new List<Vector3Int>();
        foreach (var node in path)
        {
            pathVectors.Add((Vector3Int)node.tile.position);
        }

        //tooltipOverlayController.Clear();
        tooltipOverlayController.PaintArea(pathVectors.ToArray() , overlayController.yellow);
    }

    public WorldTile GetMouseTile()
    {
        if(IsTileExist(mouseGridPosition)){
            return GetTile(mouseGridPosition);
        }
        return null;
    }

    internal WorldTile GetTile(Vector3Int position)
    {
        if(!IsTileExist(position)) return null;
        return world[position.x , position.y];
    }

    public WorldTile GetRandomTile(){
        return GetTile(GetRandomTilePosition());
    }
    internal Vector3Int GetRandomTilePosition()
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
        Vector3Int pos = GetRandomTilePosition();
        if(world[pos.x ,pos.y].CreatureID == 0 ||!world[pos.x ,pos.y].walkable){
            return pos;
        }
        return GetRandomEmptyTile(iterations-1);
    }




    internal void AddWorkingTile(WorldTile tile)
    {
        Player.Main.foodPoints.income += tile.foodOutput;
        Player.Main.industryPoints.income +=tile.industryOutput;
        Player.Main.magicPoints.income += tile.magicOutput;

        Player.Main.foodPoints.value += tile.foodOutput;
        Player.Main.industryPoints.value += tile.industryOutput;
        Player.Main.magicPoints.value += tile.magicOutput;

    }
    void OnMouseClick(bool isLeftClick){
        Vector3Int gridPosition = mouseGridPosition;

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
        Vector2Int max = new Vector2Int(world.GetLength(0) -1, world.GetLength(1) -1);

        if(position.x < 0 || position.x > max.x) return false;
        if(position.y < 0 || position.y > max.y) return false;
        return true; 
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

    public static int DistanceOf(Vector2Int a , Vector2Int b){
        return DistanceOf((Vector3Int)a , (Vector3Int)b);
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

    public Vector3 MapToWorldPosition(Vector3Int mapPosition){
        return map.CellToWorld(mapPosition);
    }

    internal TileData GetTileData(TileFeature feature)
    {
        var arr = worldGenData.tileGenDefinition;
        foreach (var data in arr)
        {
            if(data.feature == feature){
                return data;
            }
        }
        //fallback option
        Debug.LogWarning($"Can't find feature of type {feature.ToString()}, using fallback option.");
        return arr[0];
    }

    public void SetTile(WorldTile tile){
        if(!IsTileExist((Vector3Int)tile.position)){
            Debug.LogError($"Can't place a tile outside of world borders in position {tile.position}");
            return;
        }
        TileData data = GetTileData(tile.feature);
        map.SetTile((Vector3Int)tile.position , data.tile);
        try{
            world[tile.position.x , tile.position.y] = tile;
        }
        catch(IndexOutOfRangeException ){
            Debug.LogError(tile.position);
        }
    }
    private Vector3Int GetMouseGridPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Vector3Int.RoundToInt(Input.mousePosition));
        Vector3Int gridPosition = map.WorldToCell(new Vector3(mousePosition.x , mousePosition.y , 0));
        return gridPosition;
    }
}
