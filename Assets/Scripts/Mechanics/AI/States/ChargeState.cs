using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class ChargeState : State
    {
        public override void Activate(CreatureAgent agent)
        {
            WorldTile tile = agent.GetFavorableTile();

            if(tile.CreatureID == 0){
                agent.creature.UpdatePosition((Vector3Int)tile.position);
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

        public override float GetPositionScore(WorldTile tile, int depth ,int creatureID)
        {
            if (depth <= 0) return 0;

            if (tile.CreatureID != 0)
            {
                return 0;
            }

            Creature creature = Creature.GetCreature(creatureID);

            //unreachable tile
            if (!tile.walkable && !creature.flying) return 0;

            //Normal tile, look at its neighbor avarage

            WorldTile[] tiles = tile.GetTilesInRange(creature.attackRange);
            float sum = 0;
            foreach (WorldTile t in tiles)
            {
                if (t.CreatureID != 0)
                {
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if (other.Player.IsMain())
                    {
                        // tile is next to creature. Move to attack
                        //Favour keeping distance as possible
                        return WorldController.DistanceOf((Vector3Int)t.position , (Vector3Int)tile.position);
                    }
                }

                sum += GetPositionScore(t, depth - 1 , creatureID);
            }
            float avarage = sum / tiles.Length;
            float res = avarage + UnityEngine.Random.Range(0, .25f);
            return res;
        }
    }
}