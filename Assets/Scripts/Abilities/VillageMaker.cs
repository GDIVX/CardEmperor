using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMaker : CreatureMaker
{
    public static Dictionary<Vector3Int , Creature> villages = new Dictionary<Vector3Int, Creature>();
    public override void Activate(Vector3Int targetPosition)
    {
        amphibious = true;
        base.Activate(targetPosition);

        WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];
        
        WorldController.Instance.AddWorkingTile(tile);

        villages.Add(targetPosition , creature);
    }
}
