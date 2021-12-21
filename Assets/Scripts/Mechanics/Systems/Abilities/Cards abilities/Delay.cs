using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class Delay : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return true;
    }

    protected override void OnStart()
    {
        
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {       //Create a portal
            Vector3Int position = WorldController.Instance.GetRandomEmptyTile();
            //double check if the position is empty
            if(position.z == -1) return false;
            Card card = Card.BuildCard("Portal" , Player.Rival.ID);
            card.ability.Activate(position);

            Player.Rival.Boss.creature.AddEffect(new Chained(card.data.parm1));

            HandleRemoval(ID);

            return true;
    }
}
