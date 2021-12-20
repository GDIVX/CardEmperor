using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Systems.Players
{
    public abstract class Player
    {

        public int ID { get { return _ID; } }
        public int cardsToDraw = 5;
        public static MainPLayer Main { get { return _main; } }
        public static Rival Rival { get { return _rival; } }
        //manas
        public Mana foodPoints, industryPoints, magicPoints;

        public Action OnTurnStartDelegate;
        public Action OnTurnEndDelegate;

        protected int _ID;
        protected static MainPLayer _main;
        protected static Rival _rival;
        protected static Dictionary<int, Player> PlayersRegestry = new Dictionary<int, Player>();

        public static Player GetPlayer(int ID)
        {
            return PlayersRegestry[ID];
        }

        public bool IsMain()
        {
            return this == Main;
        }

        public abstract void OnTurnStart();


        public abstract void OnTurnEnd();

        public override string ToString()
        {
            return IsMain() ? "Main Player" : "Rival Player";
        }
    }
}