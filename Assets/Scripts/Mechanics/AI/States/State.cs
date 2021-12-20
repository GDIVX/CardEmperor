using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    [System.Serializable]
    public abstract class State
    {
        public abstract State Activate(CreatureAgent agent);

        protected bool CanAttack(Creature target , Creature creature)
        {
            if(target == null || creature == null) return false;
            
            if(creature.attacksAttempts >= creature.attacksPerTurn) return false;

            int distance = WorldController.DistanceOf(target.position , creature.position);
            return (distance <= creature.attackRange);
        }

        protected Creature FindTarget(WorldTile tile , int attackRange){
            WorldTile[] tilesInRange = tile.GetTilesInRange(attackRange);

            foreach (var t in tilesInRange)
            {
                if(t.CreatureID !=0){
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if(other.Player.IsMain()){
                        return other;
                    }
                }
            }
            return null;
        }
    }
}