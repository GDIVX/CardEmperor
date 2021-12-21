using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

namespace Assets.Scripts.Mechanics.AI
{
    public abstract class BossAgent : CreatureAgent
    {
        public Action OnBossDeafetedDelegate;

        protected string lastCardPlayed;
        public BossAgent()
        {
            if(Player.Rival.Boss == null){
                Player.Rival.Boss = this;
            }
            Creature.OnCreatureDeath += OnBossDeafeted;
        }

        public override void OnTurnStart()
        {
            if(creature.speed > 0){
                base.OnTurnStart();
                AddCursedCardToDrawPile();
            }

        }

        void OnBossDeafeted(Creature creature){
            if(this.creature == creature){
                OnBossDeafetedDelegate?.Invoke();
            }
        }

        protected abstract void AddCursedCardToDrawPile();
    }
}