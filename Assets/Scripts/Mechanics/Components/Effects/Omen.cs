using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Omen : Effect
    {
        public Omen(int value) : base(value)
        {
            UIData = GetData("Omen");
        }

        protected override void OnRemoved()
        {

        }

        protected override void _OnCreated()
        {
            if(value >= 10){
                Card card = new Card(CardsMannager.Instance.deck.reseveCard , Player.Main.ID);
                CardsMannager.Instance.hand.AddCard(card);
                value = 0;
                Remove();
            }
        }

        protected override void _OnTurnEnd()
        {

        }
    }
}