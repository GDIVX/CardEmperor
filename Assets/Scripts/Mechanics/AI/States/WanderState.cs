using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    internal class WanderState : State
    {
        public override void Activate(CreatureAgent agent)
        {
            WorldTile tile = agent.GetFavorableTile();

            if(tile.CreatureID == 0){
                agent.creature.UpdatePosition((Vector3Int)tile.position);
            }
            //agent.OnStateActivatedDone();
        }

        public override float GetPositionScore(WorldTile tile, int depth , int creatureID)
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

            WorldTile[] tiles = tile.GetNeighbors();
            float sum = 0;
            foreach (WorldTile t in tiles)
            {
                if (t.CreatureID != 0)
                {
                    Creature other = Creature.GetCreature(t.CreatureID);
                    if (other.Player.IsMain())
                    {
                        // tile is next to creature. Move to attack
                        return 1;
                    }
                }

                sum += GetPositionScore(t, depth - 1 , creatureID);
            }
            float avarage = sum / tiles.Length;
            float res = avarage;
            return res;
        }
    }
}