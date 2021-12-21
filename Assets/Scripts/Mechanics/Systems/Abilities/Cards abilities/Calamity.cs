using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;
using Random = UnityEngine.Random;

public class Calamity : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return true;
    }

    protected override void OnStart()
    {
        GameManager.Instance.turnSequenceMannager.OnTurnComplete += OnTurnEnd;
        isExiled = false;
    }

    bool isExiled = false;
    private void OnTurnEnd(Turn turn)
    {
        if(isExiled) return;
        if(turn.player == Player.Main){
            //pick a random creature of the main player
            var creatures = Creature.GetAllCreaturesOfPlayer(Player.Main.ID);
            Creature creature = creatures[Random.Range(0 , creatures.Length)];

            //deals _X_ damage
            Card card = Card.GetCard(ID);
            creature.TakeDamage(card.data.parm1);

            //add a copy of this card to the draw pile
            Card copy = Card.Copy(card , Player.Main.ID);
            CardsMannager.Instance.drawPile.Drop(copy);
            CardsMannager.Instance.drawPile.Shuffle();

            HandleRemoval(ID);

        }
    }


    protected override bool _Activate(Vector3Int targetPosition)
    {
        RemoveAndExile(ID);
        isExiled = true;
        return true;
    }
}
