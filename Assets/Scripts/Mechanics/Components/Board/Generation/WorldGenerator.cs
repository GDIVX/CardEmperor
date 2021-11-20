using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGenerator 
{
    public static void GenerateWorld(WorldGenData data , Tilemap map){
        
        int seed = GenerateSeed(data);

        float[,] noiseMap = Noise.GenerateNoiseMap(
            data.size.x , data.size.y,
            seed,
            data.scale,
            data.octaves,
            data.persistance, 
            data.lacunarity,
            data.offset,
            data.normalizeMode
        );

        map.ClearAllTiles();

        if(WorldController.Instance.world == null){
            WorldController.Instance.world = GenerateWorldData(noiseMap , data);
            //WorldController.Instance.world = UpdateWorldData(WorldController.Instance.world , data);
        }
        
        AddManaNodes(data);

        UpdateWorldVisuals(new RectInt(0 , 0 , data.size.x ,data.size.y) , data , map );

    }


    private static int GenerateSeed(WorldGenData data){
        return data.seed == 0 ? Random.Range(-9999,9999) : data.seed;
    }

    public static WorldTile[,] GenerateWorldData(float[,] noiseMap, WorldGenData data){

        WorldTile[,] worldData = new WorldTile[data.size.x , data.size.y];

        for (int x = 0; x < data.size.x; x++)
        {
            for (int y = 0; y < data.size.y; y++)
            {
                float hight = noiseMap[x,y];
                TileGenDefinition[] definitions = data.tileGenDefinition;
                for (int i = 0; i < definitions.Length; i++)
                {
                    if(hight <= definitions[i].maxhight){
                        TileFeature feature = definitions[i].feature;
                        worldData[x,y] = new WorldTile(new Vector2Int(x,y) , feature , definitions[i].walkable);
                        break;
                    }
                    worldData[x,y] = new WorldTile(new Vector2Int(x,y) , definitions[definitions.Length-1].feature , definitions[definitions.Length-1].walkable);
                }
            }
        }

        return worldData;
    }

    private static void AddManaNodes( WorldGenData data)
    {
        List<WorldTile> landTiles = new List<WorldTile>();

        //save the center for the capital
        int hX = Mathf.RoundToInt(data.size.x / 2);
        int hY = Mathf.RoundToInt(data.size.y / 2);
        WorldTile savedTile = WorldController.Instance.world[hX,hY];
        landTiles.Remove(savedTile);
        foreach (var t in savedTile.GetNeighbors())
        {
            landTiles.Remove(t);
        }


        //Create a list of walkable tiles
        for (var x = 0; x < data.size.x; x++)
        {
            for (var y = 0; y < data.size.y; y++)
            {
                if(WorldController.Instance.world[x,y].walkable){
                    landTiles.Add(WorldController.Instance.world[x,y]);
                }
            }
        }

        landTiles = GenerateNodesOfType(data.farms , data.nodesGenDefinitions[0] , landTiles);
        landTiles = GenerateNodesOfType(data.forests , data.nodesGenDefinitions[1], landTiles);
        GenerateNodesOfType(data.orbs  , data.nodesGenDefinitions[2], landTiles);
    }

    private static List<WorldTile> GenerateNodesOfType(int amount , NodesGenDefinition nodesGenDefinition, List<WorldTile> landTiles)
    {
        while(amount > 0){
            int rand = Mathf.RoundToInt(Random.Range(0 , landTiles.Count));
            WorldTile tile = landTiles[rand];

            tile.feature = nodesGenDefinition.heartFeature;
            tile.walkable = false;
            landTiles.Remove(tile);
            foreach (var n in tile.GetNeighbors())
            {
                n.feature = nodesGenDefinition.nodesFeature;
                landTiles.Remove(n);
            }
            amount--;
        }

        return landTiles;
    }

    public static void UpdateWorldVisuals(RectInt area , WorldGenData data , Tilemap map ){
        

        TileGenDefinition[] definitions = data.tileGenDefinition;
        NodesGenDefinition[] nodesDef = data.nodesGenDefinitions;

        for(int x = area.xMin ; x < area.xMax ; x++){
            for(int y = area.yMin ; y < area.yMax ; y++ ){

                TileFeature feature = WorldController.Instance.world[x,y].feature;
                TileBase tile = null;

                //Loop trough all definitions to find the one that match
                foreach(TileGenDefinition def in definitions){
                    if(feature == def.feature){
                        tile = def.tile;
                        break;
                    }
                }
                if(tile == null){
                    foreach (var def in nodesDef)
                    {
                        if(feature == def.heartFeature){
                            tile = def.heartTile;
                            break;
                        }
                        if(feature == def.nodesFeature){
                            tile = def.nodesTile;
                            break;
                        }
                    }
                }

                if(tile == null){Debug.LogWarning("Can't find a tile for visuals update"); return;}

                //update tilebase
                map.SetTile(new Vector3Int(x,y,0) , tile);
            }
        }     
    }
}
