using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{

    public int ID{get{return _ID;}}
    public int cardsToDraw = 5;
    public static Player Main{get{return _main;}}
    public static Player Rival{get{return _rival;}}
    //manas
    public Mana foodPoints, industryPoints , magicPoints , knowledge;  

    private int _ID;
    private static Player _main;
    private static Player _rival;
    private static Dictionary<int, Player> PlayersRegestry = new Dictionary<int, Player>(); 

    public Player(bool isMain = false){
        _ID = IDFactory.GetUniqueID();
        Player.PlayersRegestry.Add(ID , this);

        if(isMain){
            Player._main = this;
        }
        else{
            Player._rival = this;
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
        // string deb = turn == null ? "Skip Turn" : (turn.player.IsMain() ? "Player Turn" : "Rival Turn");
        // Debug.Log(deb);

        if(IsMain()){
            StartPlayerTurn();
        }
        else{
            StartRivalTurn();
        }

    }

    private void StartRivalTurn()
    {
        Debug.Log("Rival Turn");
        GameManager.Instance.turnSequenceMannager.EndTurn();
    }

    private void StartPlayerTurn()
    {
        foodPoints.SetValue(foodPoints.income);
        industryPoints.SetValue(industryPoints.income);
        magicPoints.SetValue(magicPoints.income);
        knowledge.SetValue(knowledge.Value + knowledge.income);
    }

    public void OnTurnEnd(){
        if(IsMain()){
            foodPoints.SetValue(0);
            industryPoints.SetValue(0);
            magicPoints.SetValue(0);    
        }
        else{
            GameManager.Instance.turnSequenceMannager.NextTurn();
        }
    }

    public override string ToString()
    {
        return IsMain()? "Main Player" : "Rival Player";
    }
}
