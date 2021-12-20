using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Chained : Effect
    {
        int originalSpeed;
        Creature creature;
        public Chained(int value) : base(value)
        {
            UIData = GetData("Chained");
        }

        protected override void OnRemoved()
        {
            
        }

        protected override void _OnCreated()
        {
            creature = Creature.GetCreature(creatureID);

            originalSpeed = creature.speed;
            creature.speed = 0;
        }

        protected override void _OnTurnEnd()
        {
            value--;
            if(value <= 0){
                creature.speed = originalSpeed;
            }
        }
    }
}