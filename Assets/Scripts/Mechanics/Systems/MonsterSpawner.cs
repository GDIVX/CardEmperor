using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public static class MonsterSpawner 
{
    public static void Spawn(string creatureName , Vector3Int position, CreatureAgent agent){
        Creature creature = Creature.BuildAndSpawnCardless(creatureName , Player.Rival.ID , position);
        agent.SetID(creature.ID);
        Player.Rival.Agents.Add(agent);
    }

    internal static void SpawnNew()
    {
    }
}
