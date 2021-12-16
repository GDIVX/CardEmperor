using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public class WanderAgent : CreatureAgent
    {
        State state;
        public override void OnTurnStart()
        {
            if(state == null){
                state = new IdleState();
            }

            WorldTile tile = WorldController.Instance.world[creature.position.x , creature.position.y];
            state = state.GetNextState(tile , creature.ID);
            state.Activate(this);
        }

        protected override float GetPositionScore(WorldTile tile, int depth)
        {
            return state.GetPositionScore(tile , depth , creature.ID);
        }
    }
}