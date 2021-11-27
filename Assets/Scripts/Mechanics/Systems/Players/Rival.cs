using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Systems.Players
{
    public class Rival : Player
    {
        public List<CreatureAgent> Agents = new List<CreatureAgent>();

        public Personality personality => GetPersonality();
        Personality _personality;
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
            personality.Play();

            foreach (var agent in Agents)
            {
                agent.OnTurnStart();
            }
            
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }

        Personality GetPersonality(){
            if(_personality == null){
                _personality = PersonalityFactory.Generate();
            }
            return _personality;
        }
    }
}