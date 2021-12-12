using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class PortalMaker : CreatureMaker
{

    protected override bool _Activate(Vector3Int targetPosition)
    {
        if(base._Activate(targetPosition)){
            creature.AddEffect(new Emerge(2));
            return true;
        } 
        return false;
    }
}
