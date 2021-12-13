using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public abstract class State
    {
        public abstract State GetNextState(WorldTile tile , int creatureID);
        public abstract float GetPositionScore(WorldTile tile , int depth , int creatureID);
        public abstract void Activate(CreatureAgent agent);
    }
}