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

        public override float GetPositionScore(WorldTile tile, int depth, int CreatureID)
        {
            return 0;
        }
    }
}