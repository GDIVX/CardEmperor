using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand 
{

    public int Count{get => cardsIDs.Count;}
    public List<int> cardsIDs { get => _cardsIDs; set => _cardsIDs = value; }

    List<int> _cardsIDs = new List<int>();
    HandGUI GUI;

        public Hand(){
        GUI = UIController.Instance.handGUI;
    }



    public void AddCard(Card card){
        if(card == null){
            Debug.LogWarning("Trying to add a null card to hand");  
            return;
        }
        cardsIDs.Add(card.ID);
        GUI.AddCard(card);
    }

    internal int GetCardIDByIndex(int index)
    {
        return GetCardByIndex(index).ID;
    }

    internal Card GetCardByIndex(int index)
    {
        return Card.GetCard(cardsIDs[index]);
    }

    public bool IsEmpty(){
        return Count <= 0;
    }

    internal void RemoveCard(int ID)
    {
        if(!Has(ID)){
            Debug.LogError("Trying to remove a card that is not in the hand");
            return;
        }
        cardsIDs.Remove(ID);
        GUI.RemoveCard(ID);
    }




    public bool Has(int ID){
        return cardsIDs.Contains(ID);
    }



    
}
