using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldTileFactory 
{
    public static WorldTile GenerateTile(Vector2Int position , TileFeature feature){
        TileData data = WorldController.Instance.GetTileData(feature);
        WorldTile tile = new WorldTile(position , feature , data.walkable);

        tile.speedCost = data.speedCost;
        tile.foodOutput = data.foodOutput;
        tile.industryOutput = data.industryOutput;
        tile.magicOutput = data.magicOutput;
        tile.armorBonus = data.armorBonus;
        tile.attackBonus = data.attackBonus;

        return tile;
    }
}
