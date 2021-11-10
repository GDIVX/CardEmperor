using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{

    public int ID{get{return _ID;}}
    public int cardsToDraw = 5;
    public static Player Main{get{return _main;}}
    //manas
    public Mana foodPoints, industryPoints , magicPoints , knowledge;  

    private int _ID;
    private static Player _main;
    private static Dictionary<int, Player> PlayersRegestry = new Dictionary<int, Player>(); 

    public Player(bool isMain = false){
        _ID = IDFactory.GetUniqueID();
        Player.PlayersRegestry.Add(ID , this);

        if(isMain){
            Player._main = this;
        }

        foodPoints = new Mana(ManaType.FOOD);
        industryPoints = new Mana(ManaType.INDUSTRY);
        magicPoints = new Mana(ManaType.MAGIC);
        knowledge = new Mana(ManaType.KNOWLEDGE);


    }

    public static Player GetPlayer(int ID){
        return PlayersRegestry[ID];
    }

    public bool IsMain(){
        return this == Player.Main;
    }

    public void OnTurnStart()
    {
        foodPoints.SetValue(foodPoints.income);
        industryPoints.SetValue(industryPoints.income);
        magicPoints.SetValue(magicPoints.income);
        knowledge.SetValue(knowledge.Value + knowledge.income);
    }

    public void OnTurnEnd(){
        foodPoints.SetValue(0);
        industryPoints.SetValue(0);
        magicPoints.SetValue(0);    
    }
}
