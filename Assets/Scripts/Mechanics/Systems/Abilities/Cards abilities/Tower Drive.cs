using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class TowerDrive : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return false;
    }

    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        Card card = Card.GetCard(ID);

        for (var i = 0; i <= card.data.parm1; i++)
        {
            Card towerCard = Card.BuildCard("Tower", Player.Main.ID);
            if (towerCard == null)
            {
                Debug.LogError("Failed to build card");
                return false;
            }
            CardsMannager.Instance.hand.AddCard(towerCard);
        }

        HandleRemoval(ID);
        return true;
    }
}
