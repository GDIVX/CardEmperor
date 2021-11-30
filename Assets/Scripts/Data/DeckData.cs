using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(menuName = "Cards/Deck Data")]
public class DeckData : ScriptableObject
{
    public List<CardData> starterDeck = new List<CardData>();
    public List<CardData> commonCards = new List<CardData>();
    public List<CardData> uncommonCards = new List<CardData>();
    public List<CardData> rareCards = new List<CardData>();
    public List<CardData> exileCards = new List<CardData>();

    public CardData Reserve;
    public Stack<CardData> ExtractDeck(){
        return new Stack<CardData>(starterDeck);
    }


}