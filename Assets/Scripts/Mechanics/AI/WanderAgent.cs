using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAgent : CreatureAgent
{

    public override void OnTurnStart()
    {
        if(agentState == AgentState.moving){
            HandleMoveState();
        }
        if(agentState == AgentState.attacking){
            HandleAttackingState();
        }
    }

    private void HandleAttackingState()
    {
        Creature creature = Creature.GetCreature(ID);
        WorldTile currentTile = WorldController.Instance.world[creature.position.x , creature.position.y];
        WorldTile[] tiles = currentTile.GetTilesInRange(creature.attackRange);

        foreach (var tile in tiles)
        {
            if(tile.CreatureID != 0){
                Creature other = Creature.GetCreature(tile.CreatureID);
                if(other.Player.IsMain());{
                    creature.InteractWithCreature(other);
                }
            }
        }
    }

    void HandleMoveState(){
        WorldTile tile = GetFavorableTile();
        if(tile.CreatureID != 0){
            if(tile.CreatureID == creature.ID){
                return;
            }
            agentState = AgentState.attacking;
            creature.InteractWithCreature(Creature.GetCreature(tile.CreatureID));
        }
        else{
            creature.UpdatePosition((Vector3Int)tile.position);
        }
    }
    
    protected override float GetPositionScore(WorldTile tile , int depth)
    {
        if(depth <=0)return 0;

        if(tile.CreatureID != 0){
            return 0;
        }
        //unreachable tile
        if(!tile.walkable && !creature.flying) return 0;

        //Normal tile, look at its neighbor avarage

        WorldTile[] tiles = tile.GetNeighbors();
        float sum = 0;
        foreach (WorldTile t in tiles)
        {
            if(t.CreatureID != 0){
                Creature other = Creature.GetCreature(t.CreatureID);
                if(other.Player.IsMain()){
                    // tile is next to creature. Move to attack
                    agentState = AgentState.attacking;
                    return 1;
                }
            }

            sum += GetPositionScore(t , depth-1);
        }
        float avarage = sum / tiles.Length;
        float res = avarage +  Random.Range(0,.25f);
        return res;
        
    }
}
