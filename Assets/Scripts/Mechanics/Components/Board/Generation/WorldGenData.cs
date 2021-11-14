using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WorldGenData", menuName = "World/WorldGenData", order = 0)]
[InlineEditor]
public class WorldGenData : ScriptableObject {
    [TabGroup("Noise")]
    public int seed = 0;
    [TabGroup("Noise")]
    public Vector2Int size;
    [TabGroup("Noise")]
    public Vector2 offset;
    [TabGroup("Noise")]
    public Noise.NormalizeMode normalizeMode;

    [TabGroup("Noise")]
    public float scale;
    [TabGroup("Noise")]
    public int octaves;
    [TabGroup("Noise")]
    public float persistance;
    [TabGroup("Noise")]
    public float lacunarity;


[TabGroup("Tiles Definitions")]
    public TileGenDefinition[] tileGenDefinition;
[TabGroup("Tiles Definitions")]
    public TileGenFeaturesDefinition[] tileFeaturesDefinitions;
[TabGroup("Tiles Definitions")]
[PreviewField]
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
    [VerticalGroup("Tile")]
    [PreviewField]
    public TileBase tile;
    [VerticalGroup("Tile")]
    [PreviewField]
    public Sprite overlay;
    [VerticalGroup("Tile")]
    public TileFeature feature;

        [Range(0,1)]
    public float coverage;
    [Range(-1,1)]
    public float density;
    [Range(-1,1)]
    public float sparceness;
}
