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

        protected override void _OnCreated()
        {
            Creature.GetCreature(creatureID).blockBonus = value;
        }

        protected override void _OnTurnEnd()
        {
            value--;
            Creature.GetCreature(creatureID).blockBonus = value;
            if(value <= 0){
                Remove();
            }
        }
    }
}