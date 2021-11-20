using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WorldGenData", menuName = "World/WorldGenData", order = 0)]
[InlineEditor]
public class WorldGenData : ScriptableObject {
    [TabGroup("Base")]
    public int seed = 0;
    [TabGroup("Base")]
    public Vector2Int size , Padding;
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
[TabGroup("Mana Nodes")]
    public NodesGenDefinition[] nodesGenDefinitions;
[PreviewField]
    public TileBase teritoryTile;

    [TabGroup("Mana Nodes")]
    public int farms , forests , orbs;
}

[System.Serializable]
public class TileGenDefinition{
    public TileBase tile;
    public float maxhight;
    public TileFeature feature;
    public bool walkable;
}

[System.Serializable]
public class NodesGenDefinition{
    public TileBase heartTile;
    public TileBase nodesTile;
    public int foodOutput , industryOutput , magicOutput;
    public TileFeature heartFeature , nodesFeature;
}


