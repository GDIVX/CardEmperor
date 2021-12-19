using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public class WanderAgent : CreatureAgent
    {
        public override State GetNextState(WorldTile tile)
        
        {
            //can attack now?
            if(CanAttack(tile)){
                return new AttackState();
            }

            //can move to attack?
            if(CanCharge(tile)){
                return new ChargeState();
            }

            if(creature.movement > 0)
                return new WanderState();

            return null;
        }

        private bool CanCharge(WorldTile tile)
        {
            if(creature.movement <= 0) return false;

            WorldTile[] tilesInRange = tile.GetTilesInMovementRange(creature.movement , creature.flying);
            foreach (var t in tilesInRange)
            {
                if(CanAttack(t))
                    return true;
            }
            return false;
        }

        private bool CanAttack(WorldTile tile)
        {
            if(creature.attacksAttempts >= creature.attacksPerTurn) return false;

            WorldTile[] tilesInRange = tile.GetTilesInRange(creature.attackRange);
            foreach (var t in tilesInRange)
            {
                if(t.CreatureID !=0){
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if(other.Player.IsMain()){
                        return true;
                    }
                }
            }
            return false;
        }

        protected override float GetPositionScore(WorldTile tile, int depth)
        {
            return state.GetPositionScore(tile , depth , creature.ID);
        }
    }
}