using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Spikes : Effect
    {
        public Spikes(int value) : base(value)
        {
            UIData = GetData("Spikes");
        }

        protected override void OnRemoved()
        {

        }

        protected override void _OnCreated()
        {
            GlobalDelegates.OnAttackAttempt += OnTakeDamage;
        }

        void OnTakeDamage(Creature attacker, Creature defender)
        {
            if (defender.ID == creatureID)
            {
                attacker.TakeDamage(value);
                //TODO Play effect 
            }
        }

        protected override void _OnTurnEnd()
        {

        }
    }
}