using System;
using Assets.Scripts.Mechanics.Components.Board.Pathfinding;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class ChargeState : State
    {
        public override State Activate(CreatureAgent agent)
        {
            WorldTile tile = WorldController.Instance.GetTile(agent.creature.position);

            //Do we have a target?
            if(agent.IsTargetDeadOrNull()){
                //find a target
                agent.SetTarget(FindTarget(tile , agent.creature.attackRange));
                if(agent.IsTargetDeadOrNull()){
                    //Can't find any target
                    return new WanderState().Activate(agent);
                }
                //found a target. Try to run the state again
                return Activate(agent);
            }

            //We have a target
            //Move as far as you can
            Charge(agent.target , agent.creature);

            //Can we attack now?
            if(CanAttack(agent.target , agent.creature)){
                return new AttackState().Activate(agent);
            }
            return this;

        }

        private void Charge(Creature target, Creature creature)
        {
            int movementBudget = creature.movement;
            WorldTile targetTile = WorldController.Instance.GetTile(target.position);
            WorldTile tile = WorldController.Instance.GetTile(creature.position);

            Pathfinder pathfinder = new Pathfinder();
            var path = pathfinder.FindPath(tile , targetTile , creature.flying);

            if(path == null || path.Count == 0) return;

            foreach (var node in path)
            {
                movementBudget -= node.tile.speedCost;
                int distance = WorldController.DistanceOf(node.tile.position , targetTile.position);
                //Stop when can't move or in attack range
                if(movementBudget >= 0 || distance > creature.attackRange){
                    targetTile = node.tile;
                } 
                else{
                    break;
                }
            }

            creature.MoveTo(targetTile);
        }
    }
}