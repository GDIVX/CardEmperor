using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Heal : Effect
    {
        public Heal(int value) : base(value)
        {
            UIData = GetData("Heal");
        }

        protected override void _OnCreated()
        {
            Creature creature = Creature.GetCreature(creatureID);

            creature.hitpoints += value;
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