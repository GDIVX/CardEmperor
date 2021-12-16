using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : CardAbility
{
    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);

        if (creature == null) return false;

        Card card = Card.GetCard(ID);

        creature.TakeDamage(card.data.parm1);

        HandleRemoval(ID);

        return true;
    }

        public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID !=0;
    }
}
