using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class AddVigor : CardAbility
{
    public override void Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);
        if(creature == null){
            return;
        }

        creature.AddEffect(new Vigor(5));
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.discardPile.Drop(Card.GetCard(ID));

    }

    public override void Activate(CardDisplayer targetCard)
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnTriggerEnabled()
    {
    }
}
