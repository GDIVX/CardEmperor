using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WorldGenData", menuName = "card test/WorldGenData", order = 0)]
public class WorldGenData : ScriptableObject {
    public int seed = 0;
    public Vector2Int size;
    public Vector2 offset;
    public Noise.NormalizeMode normalizeMode;

    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;



    public TileGenDefinition[] tileGenDefinition;
    public TileGenFeaturesDefinition[] tileFeaturesDefinitions;
    public TileBase teritoryTile;
}

[System.Serializable]
public class TileGenDefinition{
    [Range(0,1)]
    public float maxhight;
        public TileFeature feature;
}
[System.Serializable]
public class TileGenFeaturesDefinition{
    public TileBase tile;
    public Sprite overlay;
    public TileFeature feature;

        [Range(0,1)]
    public float coverage;
    [Range(-1,1)]
    public float density;
    [Range(-1,1)]
    public float sparceness;
}
