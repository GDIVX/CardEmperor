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

        protected float GetScoreBasedOnTileBonuses(WorldTile tile){
            return (tile.armorBonus * 0.2f) + (tile.attackBonus * 0.2f); 
        }
    }
}