using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Toughness : Effect
    {
        public Toughness(int value) : base(value)
        {
            UIData = GetData("Toughness");
        }

        protected override void OnRemoved()
        {
        }

        protected override void _OnCreated()
        {
            Creature creature = Creature.GetCreature(creatureID);

            creature.hitpoints += value;
        }

        protected override void _OnTurnEnd()
        {
            value--;
            Creature.GetCreature(creatureID).hitpoints--;
            if(value <= 0){
                Remove();
            }
        }
    }
}