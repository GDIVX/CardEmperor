using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class BirdsCounting : CardAbility
{
    protected override void OnStart()
    {
        
    }

    protected override void OnTriggerEnabled()
    {
        
    }

    protected override void _Activate(Vector3Int targetPosition)
    {
        GameManager.Instance.capital.AddEffect(new Omen(1));
        Creature.GetCreatureByPosition(targetPosition).AddEffect(new Aim(2));

        RemoveAndDiscard(ID);
    }

    protected override void _Activate(CardDisplayer targetCard)
    {
        
    }
}
