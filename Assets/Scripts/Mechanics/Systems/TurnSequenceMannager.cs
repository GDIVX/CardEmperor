using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;

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
    bool active = true;

    public void StartNewRound(){

        StartDay();
        NextTurn();
    }


    public void StartDay(){
        daysCount++;
        timeIndex = 0;
        turns = new Turn[2,5];

        SetTurnsTimeIndexes(Player.Main , 0 , 4);
        SetTurnsTimeIndexes(Player.Rival , 1 , 4);

        UpdateUI();
        if(daysCount > NewCardsEventDaysCount())
            GameEventMannager.FireNewCardEvent();
    }


    public void NextTurn()
    {
        if(GameEventMannager.isPlayingEvent){
            GameEventMannager.onAnyEventDone += NextTurn;
            return;
        }

        if(currentTurn != null && currentTurn.IsActive) EndTurn();

        currentTurn = SetCurrentTurn();

        if(currentTurn == null){
            NextTimeIndex();
            NextTurn();
            return;
        }

        currentTurn.Start();
        OnTurnStart?.Invoke(currentTurn);


    }

    public void EndTurn(){
        currentTurn.End();
        OnTurnComplete?.Invoke(currentTurn);
    }
    private int NewCardsEventDaysCount()
    {
        return Mathf.RoundToInt(GameManager.Instance.progressionCurve.Evaluate(GameManager.Instance.level));
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
            StartDay();
        }
        else{
            timeIndex = res;
            UIController.Instance.clockUI.MoveTo(timeIndex);
        }
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