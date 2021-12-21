using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class Burrow : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return true;
    }

    protected override void OnStart()
    {
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
            Player.Rival.Boss.creature.AddEffect(new Chained(Card.GetCard(ID).data.parm1));
            Player.Rival.Boss.creature.armor += Card.GetCard(ID).data.parm2;

            HandleRemoval(ID);

            return true;
    }
}
