﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsMannager : MonoBehaviour
{
    private static CardsMannager _instance;
    public static CardsMannager Instance{get{return _instance;}}
    public Pile discardPile{get{return _discardPile;}}
    public Pile drawPile{get{return _drawPile;}}
    public Pile ExhaustPile{get{return _exhaustPile;}}

    public DeckData deckData;
    public GameObject cardTamplate{get{return _cardTamplate;}}
    public Hand hand{get{return _hand;}}

    [SerializeField]
    GameObject _cardTamplate;

    [SerializeField]
    Hand _hand;
    Pile _drawPile;
    Pile _discardPile;
    Pile _exhaustPile;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }

        _drawPile = new Pile(GenerateDeck());
        //Create a capital card and add it to the start of the deck 
        //NOTE capital should always be the first buildable card 
        Card capitalCard = Card.BuildCard(deckData.capitalCardName , Player.Main.ID);
        _drawPile.Drop(capitalCard);

        _discardPile = new Pile();
        _exhaustPile = new Pile();

        GameManager.Instance.RegisterToTurnEnd(ClearHand);
        GameManager.Instance.RegisterToTurnStart(OnTurnStart);
    }


    private Stack<Card> GenerateDeck()
    {   
        Stack<CardData> dataBase = deckData.ExtractDeck();
        Stack<Card> res = new Stack<Card>();
        foreach (CardData data in dataBase)
        {
            res.Push(new Card(data , Player.Main.ID));
        }
        return res;
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.A)){
            DrawCards(1);
            Debug.Log($"Draw pile: {_drawPile.Size}");
        }
        if(Input.GetKeyUp(KeyCode.S)){
            DiscardCard(hand.GetCardIDByIndex(0));
            Debug.Log($"Discard Pile: {discardPile.Size}");
        }
    }

    public void ExhaustCard(int ID){
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

            _exhaustPile.Drop(card);
            return;
        }
        else{
            Debug.LogError("Trying to exhaust a card that is not in game or already in exhaust pile");
            return;
        }

        pile.Remove(card);
        _exhaustPile.Drop(card);
    }

    public void ReformPiles()
    {
        if(_discardPile.IsEmpty()){
            return;
        }
        discardPile.Shuffle();
        drawPile.TransferFrom(discardPile);
        Debug.Log("reformed piles");
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

    public void ClearHand(Player p){
        if(p.IsMain())
            {
                CardDisplayer[] displayers = hand.cardDisplayers.ToArray();
                foreach(CardDisplayer displayer in displayers){
                    int ID = displayer.ID;
                    DiscardCard(ID);
                }
            }
    }

    void OnTurnStart(Player player){
        if(player.IsMain()){
            DrawCards(player.cardsToDraw);
        }
    }
    void DrawCards(int amount){
        for (int i = 0; i < amount; i++)
        {
            hand.AddCard(_drawPile.Draw());
        }
    }


}