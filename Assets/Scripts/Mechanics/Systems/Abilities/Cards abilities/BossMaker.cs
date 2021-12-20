using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class BossMaker : CreatureMaker
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        if( base._Activate(targetPosition)){
            creature.AddEffect(new Chained(GameManager.Instance.roundsPerLevel));
            return true;
        }
        Debug.Log("Failed to spawn the boss");
        return false;
        
    }
}
