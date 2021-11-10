using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Card
{
   public int foodPrice , industryPrice , MagicPrice , hitpoint, speed, armor, attack;
   public CardData data{get{return _data;}}
      public int ID{get{return _ID;}}
      public float priority{get => _priority;}
      public int playerID{get{return _playerID;}}
      public CardAbility ability{get{return _ability;}}

      private static Dictionary<int, Card> CardsRegistry = new Dictionary<int, Card>();


   private int _ID;
   private int _playerID;

   private CardAbility abilities;

   public CardDisplayer Displayer { get => displayer;}
   private CardDisplayer displayer;
   private CardAbility _ability;
   private CardDisplayer _displayer;
   private CardData _data;
   private float _priority;

   public Card (CardData data , int playerID){

      this.foodPrice = data.foodPrice;
      this.industryPrice = data.industryPrice;
      this.MagicPrice = data.MagicPrice;
      this.hitpoint = data.creatureData.hitpoint;
      this.speed = data.creatureData.speed;
      this.armor = data.creatureData.armor;
      this.attack = data.creatureData.attack;
      this._priority = data.priority;

      _playerID = playerID;

      this._data = data;
      _ID = IDFactory.GetUniqueID();

      string abilityName = data.abilityScriptName;
      if(abilityName != null && abilityName != ""){
            _ability = System.Activator.CreateInstance(System.Type.GetType(abilityName)) as CardAbility;
            _ability.Register(ID);
      }
      Card.CardsRegistry.Add(ID , this);
   }

   public static Card GetCard(int ID){
      return Card.CardsRegistry[ID];
   }

   public static bool CardExist(int ID){
      return CardsRegistry.ContainsKey(ID);
   }
   internal static Card BuildCard(string cardName , int playerID)
   {
      if(cardName != null && cardName !="")
      {  
         CardData data = Resources.Load<CardData>($"Data/Cards/{cardName}");
         if(data == null){
            Debug.LogError($"Can't find a card data Resources/Cards/{cardName}");
            return null;
         }
         Card card = new Card(data , playerID);
         return card;
      }
      else{
         Debug.LogError($"Can't find a card with the name of {cardName}");
         return null;
      }
   }
   
}
