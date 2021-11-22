using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Systems.Players
{
    public class MainPLayer : Player
    {
                public MainPLayer()
        {
            _ID = IDFactory.GetUniqueID();
            PlayersRegestry.Add(ID, this);

            _main = this;

            foodPoints = new Mana(ManaType.FOOD);
            industryPoints = new Mana(ManaType.INDUSTRY);
            magicPoints = new Mana(ManaType.MAGIC);
        }

        public override void OnTurnEnd()
        {
            foodPoints.SetValue(0);
            industryPoints.SetValue(0);
            magicPoints.SetValue(0);

            CardsMannager.Instance.ClearHand();
        }

        public override void OnTurnStart()
        {
            foodPoints.SetValue(foodPoints.income);
            industryPoints.SetValue(industryPoints.income);
            magicPoints.SetValue(magicPoints.income);

            CardsMannager.Instance.DrawCards(cardsToDraw);
        }
    }
}