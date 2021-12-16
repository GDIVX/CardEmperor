using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Assets.Scripts.UI;
using UnityEngine;

public class Upgrade : CardAbility
{
    List<Card> options = new List<Card>();
    Card chosenCard;

    public override bool isPlayableOnTile(WorldTile tile)
    {
        return false;
    }

    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        //Get cards in hand minus this card
        List<Card> cardsInHand = CardsMannager.Instance.hand.ToList();
        cardsInHand.Remove(Card.GetCard(ID));

        if(cardsInHand.Count > 0){
            //use this parametere to determine if the delta function was successful 
            bool res = false;

            //Call an event to choose a card
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

                //Call an event to upgrade a card
                CardEvent upgradeEvent = new CardEvent("Choose an Upgrade" , options , (y)=>{

                    Card card = Card.Replace(chosenCard,y);
                    if(card == null){
                        res = false;
                        return;
                    }


                    GameEventMannager.isPlayingEvent = false;
                    GameEventMannager.onAnyEventDone?.Invoke();
                    UIController.Instance.eventWindow.Hide();
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
