using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGenerator 
{
    public static void GenerateWorld(WorldGenData data , Tilemap map ){
        
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
            WorldController.Instance.world = UpdateWorldData(WorldController.Instance.world , data);
        }
        
        UpdateWorldVisuals(new RectInt(0 , 0 , data.size.x ,data.size.y) , data , map);

    }

    private static WorldTile[,] UpdateWorldData(WorldTile[,] world, WorldGenData data)
    {
        TileFeature[,] map = new TileFeature[data.size.x , data.size.y];
        

        for (int x = 0; x < data.size.x; x++)
        {
            for (int y = 0; y < data.size.y; y++)
            {
                TileFeature feature = world[x,y].feature;
                TileFeature improved = GameManager.Instance.definitions.GetFeatureDefinition(feature).improveTo;
                //Generate Forest
                TileGenFeaturesDefinition definition = null;

                foreach (var def in data.tileFeaturesDefinitions)
                {
                    if(def.feature == improved){
                        definition = def;
                        break;
                    }
                }

                float chance = definition.coverage / 2;
                bool roll = Random.value <= chance;
                
                if(roll){
                    map[x,y] = improved;
                }
                else{
                    map[x,y] = feature;
                }
            }
        }    
        return UpdateWorldData(world , data , map);
    }

    private static WorldTile[,] UpdateWorldData(WorldTile[,] world, WorldGenData data, TileFeature[,] map)
    {
        TileFeature[,] res = map;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                TileFeature feature = world[x,y].feature;
                TileFeature improved = GameManager.Instance.definitions.GetFeatureDefinition(feature).improveTo;
                TileGenFeaturesDefinition definition = null;

                foreach (var def in data.tileFeaturesDefinitions)
                {
                    if(def.feature == feature){
                        definition = def;
                        break;
                    }
                }

                TileGenFeaturesDefinition improvedDefintion = null;
                foreach (var def in data.tileFeaturesDefinitions)
                {
                    if(def.feature == improved){
                        improvedDefintion = def;
                        break;
                    }
                }
                res[x,y] = HandleFeaturePlacement(definition, improvedDefintion , map , x , y );
            }
        } 

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                world[x,y].feature = map[x,y];
            }
        } 

        return world;
    }

    static TileFeature HandleFeaturePlacement(TileGenFeaturesDefinition definition, TileGenFeaturesDefinition ImproveDefinition , TileFeature[,] map, int x , int y ){
        float chance = ImproveDefinition.coverage / 2;
        TileFeature improved = ImproveDefinition.feature;

        WorldTile[] neighbors = WorldController.Instance.world[x,y].GetNeighbors();
        foreach (var n in neighbors)
        {
            if(map[n.position.x , n.position.y] == improved){
                chance += ImproveDefinition.density;
                break;
            }
        }
        chance -= ImproveDefinition.sparceness;


        bool roll = Random.value <= chance;
        if(roll){
            return improved;
        }
        else{
            return definition.feature;
        }
    }

    private static int GenerateSeed(WorldGenData data){
        return data.seed == 0 ? Random.Range(-9999,9999) : data.seed;
    }

    public static WorldTile[,] GenerateWorldData(float[,] noiseMap, WorldGenData data){

        WorldTile[,] worldData = new WorldTile[data.size.x , data.size.y];
        WorldController.Instance.overlays = new GameObject[data.size.x , data.size.y];

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
                        worldData[x,y] = new WorldTile(new Vector2Int(x,y) , feature);
                        break;
                    }
                    worldData[x,y] = new WorldTile(new Vector2Int(x,y) , definitions[definitions.Length-1].feature);
                }
            }
        }

        return worldData;
    }

    public static void UpdateWorldVisuals(RectInt area , WorldGenData data , Tilemap map){
        
        for(int x = area.xMin ; x < area.xMax ; x++){
            for(int y = area.yMin ; y < area.yMax ; y++ ){

                TileFeature feature = WorldController.Instance.world[x,y].feature;
                TileBase tile = null;
                Sprite overlay = null;

                //Loop trough all definitions to find the one that match
                TileGenFeaturesDefinition[] definitions = data.tileFeaturesDefinitions;
                foreach(TileGenFeaturesDefinition def in definitions){
                    if(feature == def.feature){
                        tile = def.tile;
                        overlay = def.overlay;
                        break;
                    }
                }

                if(tile == null){Debug.LogWarning("Can't find a tile for visuals update"); return;}

                //update tilebase
                map.SetTile(new Vector3Int(x,y,0) , tile);
                //update overlay


                if(overlay == null){


                    GameObject currentOverlay = WorldController.Instance.overlays[x,y];
                    if(currentOverlay != null) {
                        currentOverlay.SetActive(false);
                        WorldController.Instance.disabledOverlays.Push(currentOverlay);
                        WorldController.Instance.overlays[x,y] = null;
                    }
                }
                else{
                    GameObject model;
                    Vector3 worldPosition = map.GetCellCenterWorld(new Vector3Int(x,y,0));
                    worldPosition += new Vector3(-0.021f , 0.221f , 0);
                    if(WorldController.Instance.disabledOverlays.Count > 0){
                        model = WorldController.Instance.disabledOverlays.Pop();
                        model.transform.position = worldPosition;
                    }
                    else{
                        model = new GameObject();
                        model.AddComponent<SpriteRenderer>();
                        model = GameObject.Instantiate(model , worldPosition , Quaternion.identity);
                    }

                    SpriteRenderer renderer = model.GetComponent<SpriteRenderer>();
                    renderer.sprite = overlay;
                    WorldController.Instance.overlays[x,y] = model;
                    model.SetActive(true);
                }
            }
        }
    }
}
