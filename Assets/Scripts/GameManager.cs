using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance{get{return _instance;}}
    public static IClickable CurrentSelected;
    static GameManager _instance;

    public Player CurrentTurnOfPlayer{get{ return turnSequenceMannager.currentTurn.player;}}
    public Definitions definitions;
    public TurnSequenceMannager turnSequenceMannager {get{ return GetTurnMannager();}}

    [ShowInInspector]
    TurnSequenceMannager _turnMannager;


    private void Awake() {
        definitions.Start();
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            _instance = this;
        }

        //Create players
        new Player(true);
        new Player(false);
    }

    void Start()
    {
        turnSequenceMannager.StartNewRound();
    }

    public void EndTurnButton(){
            turnSequenceMannager.NextTurn();
        if(CurrentTurnOfPlayer.IsMain()){
        }
    }

    private TurnSequenceMannager GetTurnMannager()
    {
        if(_turnMannager == null){
            _turnMannager = new TurnSequenceMannager(); 
        }
        return _turnMannager;
    }
}
