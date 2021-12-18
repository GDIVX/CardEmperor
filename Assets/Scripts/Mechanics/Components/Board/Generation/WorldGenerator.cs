using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGenerator 
{
    public static void GenerateWorld(WorldGenData data , Tilemap map){
        
        int seed = GenerateSeed(data);
        Random.InitState(seed);

        map.ClearAllTiles();

        WorldController.Instance.world = new WorldTile[data.size.x , data.size.y ];

        //reserve the center
        Vector2Int center = new Vector2Int(Mathf.RoundToInt(data.size.x/2) ,Mathf.RoundToInt(data.size.y/2));
        WorldTile centerTile = WorldTileFactory.GenerateTile(center , TileFeature.WATER);
        WorldController.Instance.SetTile(centerTile);

    for (var x = 0; x < data.size.x; x++)
    {
        for (var y = 0; y < data.size.y; y++)
        {
            WorldTile tile = WorldTileFactory.GenerateTile(new Vector2Int(x,y) , TileFeature.PLAINS);
            WorldController.Instance.SetTile(tile);
        }
    }

        GenerateCoastlie(data);

        GenerateRiver(data);

        GenerateTiles(TileFeature.MOUNTIAN , data.mountains);
        GenerateTiles(TileFeature.FIELD , data.farms);
        GenerateTiles(TileFeature.FOREST , data.forests);
        GenerateTiles(TileFeature.ORBS , data.orbs);

        centerTile = WorldTileFactory.GenerateTile(center , TileFeature.PLAINS);
        WorldController.Instance.SetTile(centerTile);
    }

    private static void GenerateTiles(TileFeature feature , int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            GenerateTile(feature);
        }
    }

    private static void GenerateTile(TileFeature feature)
    {
        WorldTile tile = WorldController.Instance.GetRandomTile();
        if(tile.feature != TileFeature.PLAINS){
            //if the position is not valid, try again
            GenerateTile(feature);
            return;
        }

        tile = WorldTileFactory.GenerateTile(tile.position , feature);
        WorldController.Instance.SetTile(tile);
    }

    private static void GenerateRiver(WorldGenData data)
    {
        WorldTile start = WorldController.Instance.GetRandomTile();
        WorldTile end = WorldController.Instance.GetRandomTile();

        int distance = WorldController.DistanceOf((Vector3Int)start.position , (Vector3Int)end.position);
        Vector2Int center = new Vector2Int(Mathf.RoundToInt(data.size.x/2) ,Mathf.RoundToInt(data.size.y/2));

        //avoid starting in the center of the map
        //The river must be at a minimal length
        if(start.position.x == center.x
            || start.position.y == center.y
            || end.position.x == center.x
            || end.position.y == center.y
            || distance < Mathf.Min(data.size.x , data.size.y)){
            GenerateRiver(data);
            return;
        }

        GenerateRiver(start , end);
        foreach (WorldTile tile in WorldController.Instance.world)
        {
            if(tile.feature == TileFeature.SHALLOW_WATER){
                GenerateBridge(tile);
            }
        }
    }


    private static void GenerateRiver(WorldTile currentTile, WorldTile end)
    {
        List<WorldTile> res = new List<WorldTile>();
        if(currentTile.position == end.position){
            return;
        }
        //set the current tile to shalow waters or bridge
        currentTile = WorldTileFactory.GenerateTile(currentTile.position , TileFeature.SHALLOW_WATER);
        WorldController.Instance.SetTile(currentTile);

        var neighbors = currentTile.GetNeighbors();
        var list = neighbors.ToList();
        int currentDistanceToEnd = WorldController.DistanceOf((Vector3Int)currentTile.position , (Vector3Int)end.position);

        foreach (var n in neighbors)
        {
            if(n.feature == TileFeature.SHALLOW_WATER){
                list.Remove(n);
                continue;
            }
            //Remove any neighbor that is further away from the end tile then the current tile
            int distanceToEnd = WorldController.DistanceOf((Vector3Int)n.position , (Vector3Int)end.position);
            if(distanceToEnd > currentDistanceToEnd){
                list.Remove(n);
                continue;
            }
            //also remove any tile that is surrounded by rivers
            int count = 0;
            var _neighbors = n.GetNeighbors();
            foreach (var j in _neighbors)
            {
                if(count >= 2){
                    list.Remove(n);
                    break;
                }
                if(j.feature == TileFeature.SHALLOW_WATER){
                    count++;
                }
            }
        }

        if(list.Count == 0){
            //no valid options.
            return;
        }

        //choose a neighbor in random
        int rand = Random.Range(0 , list.Count);
        GenerateRiver(list[rand] , end);
    }

    private static void GenerateBridge(WorldTile currentTile)
    {
        var neighbors = currentTile.GetNeighbors();
        int count = 0;
        
        foreach (var n in neighbors)
        {
            if(n.feature == TileFeature.PLAINS){
                count++;
            }
        }
        if(count >= 2 && count <= 4 ){
            var tilesInRange = currentTile.GetTilesInRange(5);
            foreach (var t in tilesInRange)
            {
                if(t.feature == TileFeature.BRIDGE){
                    return;
                }
            }
            currentTile = WorldTileFactory.GenerateTile(currentTile.position , TileFeature.BRIDGE);
            WorldController.Instance.SetTile(currentTile);
        }

    }

    private static void GenerateLake(WorldTile centerTile)
    {
        centerTile = WorldTileFactory.GenerateTile(centerTile.position , TileFeature.WATER);
        WorldController.Instance.SetTile(centerTile);
        List<WorldTile> neighbors = centerTile.GetNeighbors().ToList();
        foreach (var n in neighbors)
        {
            SetToWaterByChance(n);
        }
    }

    private static void GenerateCoastlie(WorldGenData data)
    {
        GenerateCoastlie(new Vector2Int(0,0) , new Vector2Int(data.size.x-1 , 0));
        GenerateCoastlie(new Vector2Int(0,0) , new Vector2Int(0 , data.size.y-1));
        GenerateCoastlie( new Vector2Int(0 , data.size.y-1) , new Vector2Int(data.size.x -1, data.size.y-1));
        GenerateCoastlie(new Vector2Int(data.size.x -1, 0) , new Vector2Int(data.size.x -1, data.size.y-1));
    }

    private static void GenerateCoastlie(Vector2Int from, Vector2Int to)
    {
        for (var x = from.x; x <= to.x; x++)
        {
            for (var y = from.y; y <= to.y; y++)
            {
                if(!WorldController.Instance.IsTileExist(new Vector3Int(x,y,0))){
                    continue;
                }
                WorldTile tile = WorldController.Instance.world[x,y];
                var arr = tile.GetNeighbors();

                foreach (var n in arr)
                {
                    SetToWaterByChance(n);
                }
            }
        }
    }

    private static void SetToWaterByChance(WorldTile tile){

        float rand = Random.value;
        if(rand <= 0.15f){
            WorldTile newTile = WorldTileFactory.GenerateTile(tile.position , TileFeature.WATER);
            WorldController.Instance.SetTile(newTile);
        }
    }

    private static int GenerateSeed(WorldGenData data){
        return data.seed == 0 ? Random.Range(-9999,9999) : data.seed;
    }

}
