using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class Spearman : CreatureMaker
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        if (base._Activate(targetPosition))
        {
            creature.AddEffect(new Formation_Spikes(Card.GetCard(ID).data.parm1));
            return true;
        }
        return false;
    }
}
