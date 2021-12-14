using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Mechanics.Systems.Players;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance{get{return _instance;}}
    public static IClickable CurrentSelected;
    static GameManager _instance;

    public Creature capital;
    public int currentLevel, currentRound, roundsPerLevel;

    public Player CurrentTurnOfPlayer{get{
        if(turnSequenceMannager.currentTurn == null){return null;}
        return turnSequenceMannager.currentTurn.player;
        }}


    [InlineEditor]
    public Definitions definitions;
    [InlineEditor]
    public EnemiesSpawnTable spawnTable;
    [HideInInspector]
    public Action OnGameOver , OnGameStart;
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
    }

    internal CreatureDisplayer GetCapitalDisplayer()
    {
        return CreatureDisplayer.GetCreatureDisplayer(capital.ID);
    }

    void Start()
    {
        SceneManager.LoadScene("MainMenu" , LoadSceneMode.Additive);
    }

    AsyncOperation UIOperation;
    public void StartNewGame(){
        SceneManager.UnloadSceneAsync("MainMenu");
        UIOperation = SceneManager.LoadSceneAsync("UIScene" , LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("WorldScene" , LoadSceneMode.Additive).completed += OnWorldLoaded;

    }

    void OnWorldLoaded(AsyncOperation operation){
        //Create players
        new MainPLayer();
        new Rival();

        //Track the capital
        WorldController.Instance.Init();
        CardsMannager.Instance.Init();
        UIController.Instance.Init();

        Creature.OnCreatureDeath += CheckIfCapitalDestroyed;
        turnSequenceMannager.Init(Player.Rival);
    }
    internal void GameOver()
    {
        OnGameOver?.Invoke();
        Prompt.ToastCenter("<color=red><b>All Is Lost!</color></b>", 3 , 50);
        //TODO move to end game scene
    }
    
    public void CheckIfCapitalDestroyed(Creature c){
        if(c.ID == capital.ID){
            GameOver();
        }
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
        GameEventMannager.FireAddCardEvent();
    }

}
