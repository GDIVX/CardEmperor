using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMaker : CreatureMaker
{
    public static Dictionary<Vector3Int , Creature> villages = new Dictionary<Vector3Int, Creature>();
    protected override void _Activate(Vector3Int targetPosition)
    {
        base._Activate(targetPosition);

        WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];
        
        WorldController.Instance.AddWorkingTile(tile);

        villages.Add(targetPosition , creature);
    }
}
