using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public class IdleState : State
    {
        public override void Activate(CreatureAgent agent)
        {
            
        }
        public override State GetNextState(WorldTile tile, int creatureID)
        {
            Creature creature = Creature.GetCreature(creatureID);
            if(creature == null){
                Debug.LogError("Can't find creature");
                return null;
            }

            WorldTile[] tilesInMovementRange = tile.GetTilesInRange(creature.movement);
            foreach (var t in tilesInMovementRange)
            {
                if(t.CreatureID != 0){
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if(other.PlayerID == Player.Main.ID){
                        if(WorldController.DistanceOf(other.position , (Vector3Int)tile.position) <= creature.attackRange){
                            return new AttackState();
                        }
                        return new ChargeState();
                    }
                }
            }
            return new WanderState();
        }

        public override float GetPositionScore(WorldTile tile, int depth, int CreatureID)
        {
            return 0;
        }
    }
}