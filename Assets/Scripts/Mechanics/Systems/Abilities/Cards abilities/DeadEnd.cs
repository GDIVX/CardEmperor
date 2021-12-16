using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnd : CardAbility
{
    protected override void OnStart()
    {
        
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        int startingDamage = Card.GetCard(ID).data.parm1;
        int formationCount = GetFormationCount(targetPosition);
        int damage = startingDamage + (formationCount * Card.GetCard(ID).data.parm2);
        Creature.GetCreatureByPosition(targetPosition).TakeDamage(damage);

        HandleRemoval(ID);
        return true;
    }

        public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID !=0;
    }
}
