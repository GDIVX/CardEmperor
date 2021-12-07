using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDitchEffort : CardAbility
{
    protected override void OnStart()
    {
        
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        //Get cards in hand, not including this one
        List<Card> cardsInHand = CardsMannager.Instance.hand.ToList();
        cardsInHand.Remove(Card.GetCard(ID));

        bool isEventSuccessful = false;

        //Fire an event to choose a card
        CardEvent cardEvent = new CardEvent("Choose a card:" , cardsInHand , (x)=>{
            UIController.Instance.eventWindow.Hide();

            Card copy = Card.Copy(x , x.playerID);

            copy.ability.Activate(targetPosition);
            x.ability.Activate(targetPosition);

            ForceExile(x);

            GameEventMannager.isPlayingEvent = false;
            GameEventMannager.onAnyEventDone ?.Invoke();
            isEventSuccessful = true;
        });

        GameEventMannager.FireEvent(cardEvent);
        HandleRemoval(ID);
        return isEventSuccessful;
    }
}
