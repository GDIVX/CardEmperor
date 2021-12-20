using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public class IdleState : State
    {
        WorldTile tile;
        Creature creature;
        public override State Activate(CreatureAgent agent)
        {
            creature = agent.creature;
            tile = WorldController.Instance.GetTile(creature.position);

            agent.SetTarget(FindTarget(tile , creature.attackRange));

            if(agent.target == null){
                if(creature.movement > 0)
                    return new WanderState().Activate(agent);

                return new IdleState();

            }

            //can attack now?
            if(CanAttack(agent.target , creature)){
                return new AttackState().Activate(agent);
            }
            return new ChargeState().Activate(agent);
        }
    }
}