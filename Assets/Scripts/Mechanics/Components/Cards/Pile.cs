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
            int rand = UnityEngine.Random.Range(0 , i);

            Card temp = arr[i];
            arr[i] = arr[rand];
            arr[rand] = temp;
        }

        //run trough the pile again and bring forth high priority cards

        for(int i = arr.Length - 1; i > 1; i-- ){
            for (var j = i - 1; j > 0 ; j--)
            {
                if(arr[i].priority == 1.5f){
                        Card temp = arr[i];
                        arr[i] = arr[0];
                        arr[0] = temp;
                        break;
                }
                if(arr[i].priority > arr[j].priority){
                    float rand = UnityEngine.Random.value;
                    bool isPass = arr[i].priority >= rand;
                    if(!isPass){
                        //once the check failed, swap with the last successful check.
                        Card temp = arr[i];
                        arr[i] = arr[j+1];
                        arr[j+1] = temp;
                        break;
                    }
                }
            }
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
