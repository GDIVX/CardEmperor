using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class AttackState : State
    {
        public override State Activate(CreatureAgent agent)
        {
            WorldTile tile = WorldController.Instance.GetTile(agent.creature.position);
            if(!agent.IsTargetDeadOrNull()){
                //Can attack?
                if(CanAttack(agent.target , agent.creature)){
                    //Attack
                    agent.creature.InteractWithCreature(agent.target);
                    //Try to attack again
                    //If it will fail for any reason, the state will be redirected 
                    return Activate(agent);
                }
                //We can't attack but we have a target
                //Are we in range?
                int distance = WorldController.DistanceOf(agent.creature.position , agent.target.position);
                if(distance <= agent.creature.attackRange){
                    //try again next turn
                    return this;
                }
                // we out of range. Charge
                return new ChargeState().Activate(agent);

            }
            else{
                //find a target
                agent.SetTarget(FindTarget(tile , agent.creature.attackRange));
                if(agent.target != null){
                    //found a target. Run the state again
                    return Activate(agent);
                }
                return new WanderState().Activate(agent);
            }
        }

    }
}