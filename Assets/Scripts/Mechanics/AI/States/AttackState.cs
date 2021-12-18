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
                Creature creature = agent.creature;
                int distance = WorldController.DistanceOf(creature.position , other.position);
                if(other.PlayerID == Player.Main.ID && distance <= creature.attackRange){
                    creature.InteractWithCreature(other);
                }
            }

            //agent.OnStateActivatedDone();
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