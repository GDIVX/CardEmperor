using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardInteractionMatrix 
{
    public static Dictionary<Vector3Int , string> GetInteractionTable(Creature creature){
        Dictionary<Vector3Int , string> res = new Dictionary<Vector3Int, string>();

        WorldTile tile = WorldController.Instance.world[creature.position.x , creature.position.y];
        var tilesInMovementRange = tile.GetTilesInMovementRange(creature.movement, creature.flying).ToList();

        tilesInMovementRange.Remove(tile);
        var tilesInAttackRange = tile.GetTilesInRange(creature.attackRange).ToList();

        //Get tiles where attack is possible 
        if(creature.attacksPerTurn > creature.attacksAttempts )
        {        
            foreach (var currentTile in tilesInAttackRange)
            {
                if(currentTile.CreatureID != 0){
                    //If the creature in the center tile is of diffrent player then this creature
                    if(Creature.GetCreature(currentTile.CreatureID).PlayerID != creature.PlayerID){
                        res.Add((Vector3Int)currentTile.position , "Red");
                        if(tilesInMovementRange.Contains(currentTile)) 
                            tilesInMovementRange.Remove(currentTile);
                    }
                } 
            }
        }

        //Get tiles for movement
        foreach (var currentTile in tilesInMovementRange)
        {
            if(currentTile.walkable && currentTile.CreatureID == 0 && !res.ContainsKey((Vector3Int)currentTile.position))
                res.Add((Vector3Int)currentTile.position , "Blue");
        }

        return res;
    }
}
