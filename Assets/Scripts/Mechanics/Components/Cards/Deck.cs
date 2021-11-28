using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    internal List<CardData> commonCards;
    internal List<CardData> uncommonCards;
    internal List<CardData> rareCards;
    private DeckData data;

    public Deck(DeckData deckData)
    {
        this.data = deckData;

        commonCards = data.commonCards;
        uncommonCards = data.uncommonCards;
        rareCards = data.rareCards;
        
    }
}
