using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class TheBegining : DealDamage
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        if (base._Activate(targetPosition))
        {
            Card card = Card.GetCard(ID);

            Card newCard = Card.BuildCard(card.data.stringParm, Player.Main.ID);
            CardsMannager.Instance.drawPile.Drop(newCard);
            CardsMannager.Instance.drawPile.Shuffle();

            RemoveAndExile(ID);

            return true;
        }

        return false;
    }
}
