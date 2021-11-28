using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using static RandomSelector;
using Random = UnityEngine.Random;

public class CardsMannager : MonoBehaviour
{
    private static CardsMannager _instance;
    public static CardsMannager Instance{get{return _instance;}}
    public Pile discardPile{get{return _discardPile;}}
    public Pile drawPile{get{return _drawPile;}}
    public Pile exilePile{get{return _exilePile;}}

    public DeckData deckData;
    public Deck deck;
    public GameObject cardTamplate{get{return _cardTamplate;}}
    public Hand hand{get{return _hand;}}

    [SerializeField]
    GameObject _cardTamplate;

    [ShowInInspector]
    Hand _hand;
    [ShowInInspector]
    Pile _drawPile;
    [ShowInInspector]
    Pile _discardPile;
    [ShowInInspector]
    Pile _exilePile;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }

        _hand = new Hand();

        _discardPile = new Pile(Pile.PileType.Discard);
        _exilePile = new Pile(Pile.PileType.Exile);


        _drawPile = new Pile(GenerateDrawPile() , Pile.PileType.Draw);
        _drawPile.Shuffle();

        deck = new Deck(deckData);
    }


    private Stack<Card> GenerateDrawPile()
    {   
        Stack<CardData> dataBase = deckData.ExtractDeck();
        Stack<Card> res = new Stack<Card>();
        foreach (CardData data in dataBase)
        {
            res.Push(new Card(data , Player.Main.ID));
        }
        return res;
    }


    public void ExileCard(int ID){
        Pile pile;
        Card card = Card.GetCard(ID);

        if(_drawPile.Has(card)){
            pile = _drawPile;
        }
        else if(_discardPile.Has(card)){
            pile = _discardPile;
        }
        else if(hand.Has(ID)){
            CardDisplayer.GetDisplayer(ID).OnDeselect();
            hand.RemoveCard(ID);

            _exilePile.Drop(card);
            return;
        }
        else{
            Debug.LogError("Trying to exhaust a card that is not in game or already in exhaust pile");
            return;
        }

        pile.Remove(card);
        _exilePile.Drop(card);
    }

    public void ReformPiles()
    {
        if(_discardPile.IsEmpty()){
            return;
        }
        discardPile.Shuffle();
        drawPile.TransferFrom(discardPile);
        
        //TODO add tweeneing animation
    }

    private void DiscardCard(int ID)
    {
        if(ID == 0){
            Debug.LogError("Trying to discard an empty displayer");
            return;
        }
        hand.RemoveCard(ID);
        _discardPile.Drop(Card.GetCard(ID));
    }

    public void ClearHand(){
        int[] IDs = hand.cardsIDs.ToArray();
        foreach(int id in IDs){
            DiscardCard(id);
        }
    }
    public void DrawCards(int amount){
        for (int i = 0; i < amount; i++)
        {
            hand.AddCard(_drawPile.Draw());
        }
    }

    public Card CreateRandomCardWithRarity(){
        Rarity rarity = GameManager.Instance.randomSelector.GetRarity();
        List<CardData> cards = null;

        switch(rarity){
            case Rarity.COMMON:
                cards = deck.commonCards;
                break;
            case Rarity.UNCOMMON:
                cards = deck.uncommonCards;
                break;
            case Rarity.RARE:
                cards = deck.rareCards;
                break;
        }

        int rand = Random.Range(0 , cards.Count);
        CardData data = cards[rand];
        return new Card(data , Player.Main.ID);
    }

    public Card CreateExileCard(){
        List<CardData> cards = deckData.exileCards;
        int rand = Random.Range(0 , cards.Count);
        CardData data = cards[rand];
        return new Card(data , Player.Main.ID);
    }
}
