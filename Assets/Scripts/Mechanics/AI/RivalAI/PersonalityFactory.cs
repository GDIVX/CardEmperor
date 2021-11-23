using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public static class PersonalityFactory 
{
    public static Personality Generate(){
        List<Decision> decisions = new List<Decision>(){

            //HOW TO MAINTAIN:
            //Add new Decision to the list
            //Use a comment to mark each one

            //Scout event
            new Decision(0 , ()=>{
                Vector3Int pos = WorldController.Instance.GetRandomTile();
                WorldTile tile = WorldController.Instance.world[pos.x , pos.y];
                WorldTile[] area = tile.GetTilesInRange(2);

                int rand = Random.Range(2,5);

                //Spawn observers in the area
                for (var i = 0; i < rand; i++)
                {
                    MonsterSpawner.Spawn("Observer" , (Vector3Int)area[Random.Range(0 , area.Length)].position , new WanderAgent());
                }
                
                Rival.Rival.personality.mood += 0.1f;
            })
        };


        return new Personality(decisions);
    }
}


