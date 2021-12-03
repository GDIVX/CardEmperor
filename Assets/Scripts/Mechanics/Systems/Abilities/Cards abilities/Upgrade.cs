using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Assets.Scripts.UI;
using UnityEngine;

public class Upgrade : CardAbility
{
    List<Card> options = new List<Card>();
    Card chosenCard;
    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        List<Card> cardsInHand = CardsMannager.Instance.hand.ToList();
        if(cardsInHand.Count > 0){
            bool res = false;
            CardEvent cardEvent = new CardEvent("Choose a card To Upgrade" , cardsInHand , (x)=>{
                List<CardData> data = x.data.UpgradeOptions;
                chosenCard = x;
                
                if(data.Count <= 0){
                    res = false;
                    return;
                }

                foreach (var _data in data)
                {
                    Card option = new Card(_data , Player.Main.ID);
                    if(option != null) options.Add(option);
                }
                if(options.Count <= 0){
                    res = false;
                    return;
                }

                //Clear the current GUI
                cardsInHand.Clear();

                CardEvent upgradeEvent = new CardEvent("Choose an Upgrade" , options , (y)=>{

                CardsMannager.Instance.hand.RemoveCard(y.ID);
                Card card = Card.Replace(chosenCard,y);
                CardsMannager.Instance.hand.AddCard(card);



                GameEventMannager.isPlayingEvent = false;
                GameEventMannager.onAnyEventDone?.Invoke();
            });

                GameEventMannager.FireEvent(upgradeEvent);
                res = true;
            });
            GameEventMannager.FireEvent(cardEvent);

            
            return res;
        }
        Prompt.ToastCenter("<color=blue>There are no upgrade options for this card</color>",1);
        return false;
    }


}
