using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMaker : CreatureMaker
{
    public override void Activate(Vector3Int targetPosition)
    {
        pioneer = true;
        base.Activate(targetPosition);

        WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];
        WorldTile[] territory = tile.GetTilesInRange(1);

        foreach (var t in territory)
        {
            WorldController.Instance.AddTerritory(t);
        }
        WorldController.Instance.AddWorkingTile(tile);
    }
}
