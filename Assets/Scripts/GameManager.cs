using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance{get{return _instance;}}
    public static IClickable CurrentSelected;
    static GameManager _instance;

    public Player CurrentTurnOfPlayer{get{ return _currentTurnOfPlayer;}}
    public Definitions definitions;
    Player _currentTurnOfPlayer;
    Action<Player> turnEndDelegate;
    Action<Player> turnStartDelegate;
    int initTasksLeft;
    bool ready = false;
    
    List<Player> turnOrder = new List<Player>();

        private void Awake() {
            NotifyOnInitTask(false);
        definitions.Start();
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            _instance = this;
        }
        turnOrder.Add(new Player(true));
        turnOrder.Add(new Player());
        _currentTurnOfPlayer = Player.Main;
            NotifyOnInitTask(true);
    }

    void Update()
    {
        if(!ready){
            return;
        }
        StartTurn(turnOrder[0]);
        ready = false;
    }

    public void EndTurnButton(){
        if(CurrentTurnOfPlayer.IsMain()){
            EndTurn();
        }
    }

    public void EndTurn()
    {
        int turnIndex = turnOrder.IndexOf(CurrentTurnOfPlayer);
        if(turnIndex >= turnOrder.Count-1){
            turnIndex = 0;
        }
        else{
            turnIndex++;
        }

        turnEndDelegate?.Invoke(CurrentTurnOfPlayer);
        CurrentTurnOfPlayer.OnTurnEnd();

        //TODO check if it is safe to pass the turn

        StartTurn(turnOrder[turnIndex]);
    }

    public void NotifyOnInitTask(bool isDone){
        initTasksLeft = isDone ? initTasksLeft - 1 : initTasksLeft + 1;
        ready = initTasksLeft <= 0;
    }

    public void RegisterToTurnEnd(Action<Player> action){
        turnEndDelegate += action;
    }
    public void RegisterToTurnStart(Action<Player> action){
        turnStartDelegate += action;
    }

    private void StartTurn(Player player)
    {
        _currentTurnOfPlayer = player;
        turnStartDelegate?.Invoke(player);
        player.OnTurnStart();
            Debug.Log("Turn Start");
        if(player.IsMain()){
            Debug.Log("Your Turn");
        }
        else{
            Debug.Log("Enemy");
            //TODO add AI :P 
            EndTurn();
        }
    }
}
