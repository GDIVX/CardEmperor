using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile 
{
    public int Size{get{return cards.Count;}}
    Stack<Card> cards;

public Pile(){
        cards = new Stack<Card>();
    }

public Pile(Stack<Card> cards){
    this.cards = cards;
        Shuffle();
    }

    public Card Draw(){
        if(cards.Count != 0)
        return cards.Pop();
        else{
            if(CardsMannager.Instance.discardPile.IsEmpty()){
                Debug.LogWarning("No more cards!");
                return null;
            }
            CardsMannager.Instance.ReformPiles();
            return CardsMannager.Instance.drawPile.Draw();
        }
    }

    public void Drop(Card card){
        if(cards == null){
            cards = new Stack<Card>();
        }

        cards.Push(card);
    }    

    public void Shuffle()
    {
        Card[] arr = cards.ToArray();

        //shuffle the pile as normal
        for(int i = arr.Length - 1; i > 0; i-- ){
            float relativePosition = 1 - arr[i].priority;
            int max = Mathf.RoundToInt((arr.Length - 1) * relativePosition);

            int rand = 0;
            if(max <= i){
                rand = UnityEngine.Random.Range(max, i);
            }
            else{
                rand = UnityEngine.Random.Range(i , max);
            }

            Card temp = arr[i];
            arr[i] = arr[rand];
            arr[rand] = temp;
        }

        //replace this deck with the new array
        cards = new Stack<Card>(arr);
    }

    internal bool IsEmpty()
    {
        return cards.Count == 0;
    }

    internal void TransferFrom(Pile otherPile)
    {
        for (int i = 0; i < otherPile.Size; i++)
        {
            cards.Push(otherPile.cards.Pop());
        }
    }

    public bool Has(Card card){
        return cards.Contains(card);
    }

    internal void Remove(Card card)
    {
        List<Card> list = new List<Card>(cards);
        list.Remove(card);
        cards = new Stack<Card>(list);
    }
}
