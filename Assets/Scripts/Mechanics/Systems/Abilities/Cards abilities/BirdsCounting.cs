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
        GameManager.Instance.capital.AddEffect(new Omen(Card.GetCard(ID).data.parm1));
        Creature.GetCreatureByPosition(targetPosition).AddEffect(new Aim(Card.GetCard(ID).data.parm2));

        HandleRemoval(ID);

        return true;
    }

        public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID !=0;
    }

}
