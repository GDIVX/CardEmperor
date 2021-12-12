using System;
using System.Collections;
using System.Collections.Generic;
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


        public override void OnTurnEnd()
        {
        }

        public override void OnTurnStart()
        {
            GameManager.Instance.currentRound++;
            Prompt.ToastCenter($"<color=blue>Start of round {GameManager.Instance.currentRound}</color>" , 2.5f , 50);
            
            if(GameManager.Instance.currentRound <= GameManager.Instance.roundsPerLevel){
                MonsterSpawner.HandleWaveSpawning();
            }

            foreach (var agent in Agents)
            {
                agent.OnTurnStart();
            }
            
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }

    }
}