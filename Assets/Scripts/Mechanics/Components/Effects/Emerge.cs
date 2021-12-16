using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.AI;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Components.Effects
{
    public class Emerge : Effect
    {
        public Emerge(int value) : base(value)
        {
            UIData = GetData("Emerge");
        }

        protected override void OnRemoved()
        {
        }

        protected override void _OnCreated()
        {
        }

        protected override void _OnTurnEnd()
        {
            value--;
            if(value <= 0){
                CardData data = GameManager.Instance.spawnTable.spawnTable[GameManager.Instance.currentLevel , Random.Range(1,5)];
                Vector3Int position = Creature.GetCreature(creatureID).position;

                Creature.GetCreature(creatureID).Kill();
                
                string agentScriptName = data.creatureData.AIAgentScriptName;
                if(agentScriptName == null || agentScriptName == ""){
                    Debug.LogError($"Agent script name is null or empty");
                    return;
                }
                CreatureAgent agent = System.Activator.CreateInstance(System.Type.GetType("Assets.Scripts.Mechanics.AI."+agentScriptName)) as CreatureAgent;
                MonsterSpawner.Spawn(data , position ,agent);
                
            }
        }
    }
}