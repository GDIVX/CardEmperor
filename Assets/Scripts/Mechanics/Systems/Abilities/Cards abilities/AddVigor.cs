using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class AddVigor : CardAbility
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);
        if(creature == null){
            return false;
        }

        creature.AddEffect(new Vigor(5));
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.discardPile.Drop(Card.GetCard(ID));

        return true;

    }


    protected override void OnStart()
    {
    }

}
