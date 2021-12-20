using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class WanderState : State
    {
        public override State Activate(CreatureAgent agent)
        {
            WorldTile tile = WorldController.Instance.GetTile(agent.creature.position);
            
            WorldTile[] tilesInMovementRange = tile.GetTilesInMovementRange(agent.creature.movement , agent.creature.flying);

            //Find the best tile
            //start with a random tile in range as a fallback option
            WorldTile targetTile = tilesInMovementRange[Random.Range(0,tilesInMovementRange.Length)];
            foreach (var t in tilesInMovementRange)
            {
                if(t.CreatureID == 0){
                    if((t.armorBonus + t.attackBonus) > (targetTile.armorBonus + targetTile.attackBonus)){
                        targetTile = t;
                    }
                }
            }

            agent.creature.MoveTo(targetTile);
            return new IdleState();
        }

    }
}