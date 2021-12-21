using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MonsterSpawner 
{
    public static void Spawn(CardData data , Vector3Int position , CreatureAgent agent){
        Card card = new Card(data , Player.Rival.ID);
        card.ability.Activate(position);
        agent.SetID(card.ID);
        Player.Rival.Agents.Add(agent);
    }

    internal static void HandleWaveSpawning()
    {
        int spawnCount = GameManager.Instance.currentRound <= 3 ? 1 : 2;

        for (var i = 0; i < spawnCount; i++)
        {
            Vector3Int position = WorldController.Instance.GetRandomEmptyTile();
            //double check if the position is empty
            if(position.z == -1) continue;
            Card card = Card.BuildCard("Portal" , Player.Rival.ID);
            card.ability.Activate(position);
        }
    }

    public static void SpawnBoss(){
        Vector3Int position = GetBossSpawnPosition();
        CardData data = GameManager.Instance.spawnTable.spawnTable[GameManager.Instance.currentLevel ,0];

        string agentScriptName = data.creatureData.AIAgentScriptName;
        if(agentScriptName == null || agentScriptName == ""){
            Debug.LogError($"Agent script name is null or empty");
            return;
        }
        Debug.Log(System.Type.GetType(agentScriptName));
        CreatureAgent agent = System.Activator.CreateInstance(System.Type.GetType(agentScriptName)) as CreatureAgent;
        MonsterSpawner.Spawn(data , position ,agent);

        //TODO Add chained effect

        //TODO link to a level won delegate
    }

    static Vector3Int GetBossSpawnPosition(){
        int x = Mathf.RoundToInt(WorldController.Instance.world.GetLength(0) / 2);
        return GetBossSpawnPosition(new Vector3Int(x,0,0));
    }

    private static Vector3Int GetBossSpawnPosition(Vector3Int position)
    {
        WorldTile tile = WorldController.Instance.world[position.x , position.y];

        if(tile.walkable && tile.CreatureID == 0 && walkableCount(tile) >= 3){
            return (Vector3Int)tile.position;
        }
        var neighbors = tile.GetNeighbors();
        WorldTile newTile = neighbors[Random.Range(0 , neighbors.Length-1)];
        return GetBossSpawnPosition((Vector3Int)newTile.position);
    }

    public static int walkableCount(WorldTile tile){
        var group = tile.GetNeighbors();
        int res = 0;
        foreach (var t in group)
        {
            if(t.walkable) res++;
        }

        return res;
    }
}
