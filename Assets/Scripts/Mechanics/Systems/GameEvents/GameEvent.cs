using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent 
{
    public string title;
    public GameEvent(string title)
    {
        this.title = title;
    }
    public Action OnInvoked;

}

public class CardEvent : GameEvent{
    public List<Card> cards;
    public Action<Card> cardAction;
    public Action action;

    public CardEvent(string title , List<Card> cards , Action<Card> OnCardSelected) : base(title)
    {
        this.cards = cards;
        this.cardAction = OnCardSelected;
    }
}
