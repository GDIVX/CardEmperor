using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card game/Deck Data")]
public class DeckData : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
    public string capitalCardName;
    public Stack<CardData> ExtractDeck(){
        return new Stack<CardData>(cards);
    }

}