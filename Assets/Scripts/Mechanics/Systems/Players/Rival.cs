using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Systems.Players
{
    public class Rival : Player
    {
        public List<CreatureAgent> Agents = new List<CreatureAgent>();

        public Personality personality;
                public Rival()
        {
            _ID = IDFactory.GetUniqueID();
            PlayersRegestry.Add(ID, this);

            _rival = this;

            personality = PersonalityFactory.Generate();

        }


        public override void OnTurnEnd()
        {
            
        }

        public override void OnTurnStart()
        {
            personality.Play();

            foreach (var agent in Agents)
            {
                agent.OnTurnStart();
            }
            
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }

    }
}