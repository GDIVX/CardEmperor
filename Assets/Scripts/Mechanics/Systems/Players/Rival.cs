using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.AI;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Systems.Players
{
    public class Rival : Player
    {
        public List<CreatureAgent> Agents = new List<CreatureAgent>();
                public Rival()
        {
            _ID = IDFactory.GetUniqueID();
            PlayersRegestry.Add(ID, this);

            _rival = this;
        }

        public BossAgent Boss { get; internal set; }

        public override void OnTurnEnd()
        {
            OnTurnEndDelegate?.Invoke();
        }

        public override void OnTurnStart()
        {
            GameManager.Instance.currentRound++;
            Prompt.ToastCenter($"<color=blue>Start of round {GameManager.Instance.currentRound}</color>" , 2.5f , 50);
            
            if(GameManager.Instance.currentRound <= GameManager.Instance.roundsPerLevel){
                MonsterSpawner.HandleWaveSpawning();
                if(GameManager.Instance.currentRound == 1){
                    MonsterSpawner.SpawnBoss();
                }
            }

            foreach (var agent in Agents)
            {
                agent.OnTurnStart();
            }

            OnTurnStartDelegate?.Invoke();
            
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }

    }
}