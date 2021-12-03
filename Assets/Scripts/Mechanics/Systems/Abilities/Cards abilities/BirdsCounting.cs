using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class BirdsCounting : CardAbility
{
    protected override void OnStart()
    {
        
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        GameManager.Instance.capital.AddEffect(new Omen(1));
        Creature.GetCreatureByPosition(targetPosition).AddEffect(new Aim(2));

        RemoveAndDiscard(ID);

        return true;
    }

}
