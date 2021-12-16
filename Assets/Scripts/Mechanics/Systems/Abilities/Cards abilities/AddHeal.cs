using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class AddHeal : CardAbility
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);
        if(creature == null){
            return false;
        }

        creature.AddEffect(new Toughness(5));
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.discardPile.Drop(Card.GetCard(ID));

        return true;
    }


    protected override void OnStart()
    {
    }

    public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID != 0;
    }
}
