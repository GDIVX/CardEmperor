using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using Assets.Scripts.Mechanics.Systems.Players;

[System.Serializable]
public class TurnSequenceMannager 
{
    public Action<Turn> OnTurnStart , OnTurnComplete;
    public int TimeIndex { get => timeIndex;}
    [TableMatrix(DrawElementMethod ="DrawElement")]
    public Turn[,] turns;
    public Turn currentTurn;    
    [ShowInInspector]
    private int timeIndex;
    public int RoundsCount = 0;
    public int daysCount = 0;

    public void StartNewRound(){

        StartDay();
        StartNextTurn();
    }


    public void StartDay(){
        daysCount++;
        turns = new Turn[2,5];

        SetTurnsTimeIndexes(Player.Main , 0 , 4);
        SetTurnsTimeIndexes(Player.Rival , 1 , 4);

        UpdateUI();
        if(daysCount > NewCardsEventDaysCount())
            GameEventMannager.FireNewCardEvent();
    }


    void StartNextTurn()
    {
        if(GameEventMannager.isPlayingEvent){
            GameEventMannager.onAnyEventDone += StartNextTurn;
            return;
        }

        if(currentTurn != null && currentTurn.IsActive){
            Debug.LogWarning("Trying to start a new turn without ending the current one");
            return;
        }

        currentTurn = SetCurrentTurn();

        if(currentTurn == null){
            NextTimeIndex();
            StartNextTurn();
            return;
        }

        currentTurn.Start();
        OnTurnStart?.Invoke(currentTurn);


    }

    void EndTurn(){
        if(currentTurn == null || !currentTurn.IsActive) return;
        currentTurn.End();
        OnTurnComplete?.Invoke(currentTurn);
    }

    public void NextTurn(){
        EndTurn();
        StartNextTurn();
    }
    private int NewCardsEventDaysCount()
    {
        return (GameManager.Instance.level * 2) + 1;
    }

    Turn SetCurrentTurn(){
        //Rival start first
        if(!IsTurnInactiveOrNull(1 , timeIndex)){
            //Rival turn
            return turns[1,timeIndex];
        }
        if(!IsTurnInactiveOrNull(0,timeIndex)){
            return turns[0,timeIndex];
        }
        return null;

    }

    private void NextTimeIndex()
    {
        int res = timeIndex+1;
        RoundsCount++;      
        //if it over 4, start the next day
        if(res > 4){
            res = 0;
            StartDay();
        }
        timeIndex = res;
        UIController.Instance.clockUI.MoveTo(timeIndex);
    }

    //Pick randomly 3 time indexes
    private void SetTurnsTimeIndexes(Player player , int playerIndex , int playerTurns){
        List<int> indexes = new List<int>{0,1,2,3,4,5};

        while(playerTurns > 0 ){
            int rand = Random.Range(0 , 5);
            if(indexes.Remove(rand)){
                turns[playerIndex , rand] = new Turn(player);
                playerTurns--;
            }
        }
    }

    private void UpdateUI()
    {
        
        UIController.Instance.clockUI.Reset();
        UIController.Instance.clockUI.MoveTo(timeIndex);
        
        for (var p = 0; p <= 1; p++)
        {
            for (var t = 0; t <= 4; t++)
            {
                Turn turn = turns[p,t];
                if(turn != null){
                    UIController.Instance.clockUI.CreateEventIcon(t , p == 0);
                }
            }
        }
    }

    public bool IsTurnInactiveOrNull(int playerIndex , int timeIndex){
        Turn turn = turns[playerIndex , timeIndex];
        return turn == null || turn.IsActive == false;
    }

    static Turn DrawElement(Rect rect , Turn value){
        Color c = (value == null || !value.IsActive) ? Color.gray :
                                (value.player.IsMain() ? Color.green : Color.red);
        if(value == GameManager.Instance.turnSequenceMannager.currentTurn){
            c += Color.blue;
        }


        EditorGUI.DrawRect(
            rect.Padding(1),
            c
        );

        return value;
    }
}

public class Turn{
    public Player player;
    private bool isActive = true;

    public bool IsActive { get => isActive;}

    public void End(){
        isActive = false;
        player.OnTurnEnd();
    }

    public void Start(){
        isActive = true;
        player.OnTurnStart();
    }


    public Turn(Player player){
        this.player = player;
    }

}