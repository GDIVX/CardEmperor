using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    [System.Serializable]
    public abstract class State
    {
        public abstract float GetPositionScore(WorldTile tile , int depth , int creatureID);
        public abstract void Activate(CreatureAgent agent);
    }
}