using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Aim : Effect
    {
        public Aim(int value) : base(value)
        {
            UIData = GetData("Aim");
        }

        protected override void _OnCreated()
        {
            Creature.GetCreature(creatureID).attack += value;
        }

        protected override void _OnTurnEnd()
        {
            Creature.GetCreature(creatureID).attack -= value;
            Remove();
        }

        protected override void OnRemoved()
        {
            
        }
    }
}