using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Mechanics.Systems.Players;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance{get{return _instance;}}
    public static IClickable CurrentSelected;
    static GameManager _instance;

    public Player CurrentTurnOfPlayer{get{
        if(turnSequenceMannager.currentTurn == null){return null;}
        return turnSequenceMannager.currentTurn.player;
        }}
    public Definitions definitions;
    public TurnSequenceMannager turnSequenceMannager {get{ return GetTurnMannager();}}
    public RandomSelector randomSelector{get{return GetRandomSelector();}}
    public int level;

    [ShowInInspector]
    TurnSequenceMannager _turnMannager;
    RandomSelector _randomSelector;

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
        new MainPLayer();
        new Rival();

    }

    void Start()
    {
        turnSequenceMannager.StartNewRound();
    }

    

    public void EndTurnButton(){
        if(CurrentTurnOfPlayer == null){return;}
        if(CurrentTurnOfPlayer.IsMain()){
            turnSequenceMannager.NextTurn();
        }
    }

    private TurnSequenceMannager GetTurnMannager()
    {
        if(_turnMannager == null){
            _turnMannager = new TurnSequenceMannager(); 
        }
        return _turnMannager;
    }

    RandomSelector GetRandomSelector(){
        if(_randomSelector == null){
            _randomSelector = GetComponent<RandomSelector>();
        }
        return _randomSelector;
    }

    [Button]
    public void debug_fireCardEvent(){
        GameEventMannager.FireNewCardEvent();
    }

    [Button]
    public void debug_spawnMonster(){
        MonsterSpawner.Spawn("Militia" , new Vector3Int(0,0,0) , new WanderAgent());
    }

}
