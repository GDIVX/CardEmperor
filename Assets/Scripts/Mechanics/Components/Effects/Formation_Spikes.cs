using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Formation_Spikes : Effect
    {
        public Formation_Spikes(int value) : base(value)
        {
            UIData = GetData("Formation_Spikes");
        }

        protected override void _OnCreated()
        {

        }

        protected override void _OnTurnEnd()

        
        {
            Creature creature = Creature.GetCreature(creatureID);
            Vector3Int targetPosition = creature.position;

            WorldTile tile = WorldController.Instance.world[targetPosition.x, targetPosition.y];
            WorldTile[] tiles = tile.GetNeighbors();
            int formationCount = 0;

            foreach (var n in tiles)
            {
                if (n.CreatureID != 0)
                {
                    if (Creature.GetCreatureByPosition((Vector3Int)n.position).Player.IsMain())
                    {
                        formationCount++;
                    }
                }
            }
            creature.SetEffect(new Spikes(value));
        }

        protected override void OnRemoved()
        {
            
        }
    }
}