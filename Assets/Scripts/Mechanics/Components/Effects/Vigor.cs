using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Vigor : Effect
    {
        public Vigor(int value) : base(value)
        {
            UIData = GetData("Vigor");
        }

        protected override void OnRemoved()
        {
            
        }

        protected override void _OnCreated()
        {
            Creature.GetCreature(creatureID).armor += value;
        }

        protected override void _OnTurnEnd()
        {
            value--;
            Creature.GetCreature(creatureID).armor--;
            if(value <= 0){
                Remove();
            }
        }
    }
}