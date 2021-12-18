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
    public Vector2Int size;


[TabGroup("Tiles Definitions")]
    public TileData[] tileGenDefinition;
[TabGroup("Mana Nodes")]

    public TileBase teritoryTile;

    [TabGroup("Base")]
    public int farms , forests , orbs , mountains;
}

[System.Serializable]
public class TileData{
    public TileBase tile;
    public TileFeature feature;
    public bool walkable;
    public int speedCost;
    public int attackBonus , armorBonus;
    public int foodOutput , industryOutput , magicOutput;

}

[System.Serializable]
public class NodesGenDefinition{
    public TileBase heartTile;
    public TileBase nodesTile;
    public int foodOutput , industryOutput , magicOutput;
    public TileFeature heartFeature , nodesFeature;
}


