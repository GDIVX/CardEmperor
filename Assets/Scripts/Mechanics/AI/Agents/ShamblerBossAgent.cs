using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Mechanics.AI;
using Assets.Scripts.Mechanics.Systems.Players;

public class ShamblerBossAgent : BossAgent
{
    protected override void AddCursedCardToDrawPile()
    {
        Card card = null;

        bool rand = .125f < Random.value;

        if(rand){
            if((creature.armor < 5 || creature.hitpoints < 15)){
                if(lastCardPlayed != "Calamity"){
                    card = Card.BuildCard("Calamity" , Player.Main.ID);
                    CardsMannager.Instance.hand.AddCard(card);
                    lastCardPlayed = "Calamity";
                    return;
                }
                if(lastCardPlayed != "Burrow"){
                    card = Card.BuildCard("Burrow" , Player.Main.ID);
                    CardsMannager.Instance.hand.AddCard(card);
                    lastCardPlayed = "Burrow";
                    return;
                }
            }

            if(Player.Main.foodPoints.income > 1){
                if(lastCardPlayed != "Infestation"){
                    card = Card.BuildCard("Infestation" , Player.Main.ID);
                    CardsMannager.Instance.discardPile.Drop(card);
                    lastCardPlayed = "Infestation";
                    return;
                }

                if(lastCardPlayed != "Hunger"){
                    card = Card.BuildCard("Hunger" , Player.Main.ID);
                    CardsMannager.Instance.hand.AddCard(card);
                    lastCardPlayed = "Hunger";
                    return;
                }
            }
        }

        card = Card.BuildCard("Delay" , Player.Main.ID);
        CardsMannager.Instance.hand.AddCard(card);
        lastCardPlayed = "Delay";
    }
}
