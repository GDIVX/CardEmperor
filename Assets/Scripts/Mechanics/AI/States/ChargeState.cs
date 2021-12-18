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
                WorldTile[] tilesInRange = tile.GetTilesInRange(agent.creature.attackRange);

                // foreach (var t in tilesInRange)
                // {
                //     if(t.CreatureID != 0){
                //         Creature other = Creature.GetCreature(t.CreatureID);
                //         if(other.Player.IsMain()){
                //             agent.states.Push(new AttackState());
                //             break;
                //         }
                //     }
                // }
            }        

            //agent.OnStateActivatedDone();
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
            }
            return 0;
        }
    }
}