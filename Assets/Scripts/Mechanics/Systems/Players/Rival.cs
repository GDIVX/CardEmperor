using System.Collections;
using System.Collections.Generic;
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
            if(Agents.Count == 0){
                MonsterSpawner.SpawnNew();
            }
            foreach (var agent in Agents)
            {
                agent.OnTurnStart();
            }
            
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }
    }
}