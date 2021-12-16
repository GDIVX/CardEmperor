using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class AttackState : State
    {
        public override void Activate(CreatureAgent agent)
        {
            for (var i = 0; i < agent.creature.attacksPerTurn; i++)
            {
                WorldTile tile = agent.GetFavorableTile();
                if(tile.CreatureID == 0) return;
                Creature other = Creature.GetCreature(tile.CreatureID);
                if(other.PlayerID == Player.Main.ID){
                    Creature creature = agent.creature;
                    creature.InteractWithCreature(other);
                }
            }
        }

        public override State GetNextState(WorldTile tile, int creatureID)
        {
            Creature creature = Creature.GetCreature(creatureID);
            if(creature == null){
                Debug.LogError("Can't find creature");
                return null;
            }

            WorldTile[] tilesInMovementRange = tile.GetTilesInRange(creature.movement);
            foreach (var t in tilesInMovementRange)
            {
                if(t.CreatureID != 0){
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if(other.PlayerID == Player.Main.ID){
                        if(WorldController.DistanceOf(other.position , (Vector3Int)tile.position) <= creature.attackRange){
                            return new AttackState();
                        }
                        return new ChargeState();
                    }
                }
            }
            return new WanderState();
        }

        public override float GetPositionScore(WorldTile tile, int depth, int creatureID)
        {

            if (tile.CreatureID == 0)
            {
                return 0;
            }

            Creature other = Creature.GetCreature(tile.CreatureID);
            if(other.PlayerID == Player.Main.ID){
                //Hostile creature 
                return 1 + Random.Range(0,.25f);
            }
            return 0;
        }
    }
}