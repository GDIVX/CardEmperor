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
}
