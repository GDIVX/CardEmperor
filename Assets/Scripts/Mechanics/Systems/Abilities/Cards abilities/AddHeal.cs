using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class AddHeal : CardAbility
{
    protected override void _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);
        if(creature == null){
            return;
        }

        creature.AddEffect(new Toughness(5));
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.discardPile.Drop(Card.GetCard(ID));

    }

    protected override void _Activate(CardDisplayer targetCard)
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnTriggerEnabled()
    {
    }
}
