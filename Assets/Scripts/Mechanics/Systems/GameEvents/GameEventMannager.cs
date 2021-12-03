using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventMannager
{
    public static Action onAnyEventDone;
    public static bool isPlayingEvent;
    private static GameEvent _FireEvent(GameEvent gameEvent)
    {
        isPlayingEvent = true;
        EventWindow eventWindow = UIController.Instance.eventWindow;
        eventWindow.Hide();
        eventWindow.SetEvent(gameEvent);
        eventWindow.Show();

        return gameEvent;
    }

    public static GameEvent FireEvent(GameEvent gameEvent){
        if(isPlayingEvent){
        EventWindow eventWindow = UIController.Instance.eventWindow;
        eventWindow.SetEvent(gameEvent);
        eventWindow.Show();
        }
        else
        {
            _FireEvent(gameEvent);
        }

        return gameEvent;
    }

    public static CardEvent FireAddCardEvent()
    {

        List<Card> cards = new List<Card>();

        for (var i = 0; i < 3; i++)
        {
            cards.Add(CardsMannager.Instance.CreateRandomCardWithRarity());
        }

        cards.Add(CardsMannager.Instance.CreateExileCard());

        CardEvent cardEvent = new CardEvent("End of the day. New forces wish to join you. Choose one:", cards, (x) =>
        {
            Card card = Card.Copy(x, x.playerID);
            CardsMannager.Instance.discardPile.Drop(card);
            CardsMannager.Instance.deck.AddCardOption(card);

            UIController.Instance.eventWindow.Hide();
            GameManager.Instance.level++;

            isPlayingEvent = false;
            onAnyEventDone?.Invoke();
        });

        return _FireEvent(cardEvent) as CardEvent;
    }



}
