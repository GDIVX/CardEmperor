using System.Diagnostics;
using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Vector3 = UnityEngine.Vector3;
using Assets.Scripts.Mechanics.Systems.Players;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class Creature: IClickable
{
    public int Hitpoint { get => hitpoint;}
    public int Hitpoint1 { get => hitpoint;}
    public int Armor { get => armor;}
    public int Speed { get => speed;}
    public Sprite icon { get => _icon;}
    public int Attack {get => attack;}
    public Vector3Int position{get => _position;}
    public int ID{get => _ID;}
    public int PlayerID{get => _PlayerID;}
    public int attackRange{get => _attackRange;}
    public int movement{get => _movement;}
    public Player Player{get{return Player.GetPlayer(PlayerID);}}
    public bool amphibious{get{return _amphibious;}}
    public bool flying{get{return _flying;}}
    public bool pioneer{get{return _pioneer;}}
    public int attacksPerTurn = 1;


    public static Action<Creature> OnCreatureDeath;


    static Dictionary<int,Creature> creaturesRegestry = new Dictionary<int, Creature>();

    [ShowInInspector]
    [ReadOnly]
    int hitpoint, armor, attack , speed, _attackRange , _ID , _PlayerID , _movement , _attacksAttempts;
    [ShowInInspector]
    [ReadOnly]
    Sprite _icon;

    [ShowInInspector]
    [ReadOnly]
    Vector3Int _position;
    [ShowInInspector]
    [ReadOnly]
    private bool _amphibious , _flying , _pioneer;
    [ShowInInspector]
    [ReadOnly]
    CreatureAbility ability;

    public Creature(CreatureData data , int cardID , Vector3Int position){
        this.hitpoint = data.hitpoint;
        this.armor = data.armor;
        this.attack = data.attack;
        this.speed = data.speed;
        this._attackRange = data.attackRange;
        this._icon = data.icon;
        _position = position;
        _movement = speed;

        _amphibious = data.amphibious;
        _flying = data.flying;
        _pioneer = data.pioneer;

        _ID = cardID;
        Card card = Card.GetCard(cardID);
        _PlayerID = card.playerID;
        creaturesRegestry.Add(cardID , this);
        WorldController.Instance.world[position.x , position.y].CreatureID = cardID;

        if(data.abilityScriptName != null && data.abilityScriptName !=""){
            ability = System.Activator.CreateInstance(System.Type.GetType(data.abilityScriptName), cardID) as CreatureAbility;
        }

        GameManager.Instance.turnSequenceMannager.OnTurnStart += OnTurnStart;
    }

    public static Creature BuildCreatureCardless(string creatureCardName , int playerID , Vector3Int position){
        Card tempCard = Card.BuildCard(creatureCardName , playerID);
        return new Creature(tempCard.data.creatureData , tempCard.ID , position);
    }

    public static Creature BuildAndSpawnCardless(string creatureCardName , int playerID , Vector3Int position){
        Creature creature = BuildCreatureCardless(creatureCardName , playerID , position);

        var worldPosition =  WorldController.Instance.map.GetCellCenterLocal(position);
        GameObject _gameObject = CreatureDisplayer.Create(creature , worldPosition);
        CreatureDisplayer displayer = _gameObject.GetComponent<CreatureDisplayer>();

        displayer.SetDisplay(true);

        return creature;
    }

    public void Kill(){
        OnCreatureDeath?.Invoke(this);
        //Clear view
        CreatureDisplayer.GetCreatureDisplayer(ID).SetDisplay(false);
        //Move the card into exile
        CardsMannager.Instance.exilePile.Drop(Card.GetCard(ID));
        //Remove the creature from the board
        WorldTile tile = WorldController.Instance.world[position.x , position.y];
        tile.CreatureID = 0;
    }
    internal void TakeDamage(int damage)
    {
        hitpoint -= damage;
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        displayer.SetDisplay(true);
        if(hitpoint <= 0){
            Kill();
        } 
    }

    internal void OnAttackBlocked(int damage)
    {
        //TODO fun animation
    }

    internal void OnAttackPassed(int damage)
    {
        //TODO Fun animation
    }

    public static Creature GetCreature(int ID){
        if(CreatureExist(ID) == false){
            UnityEngine.Debug.LogError("Trying to get a creature that dose not exist");
            return null;
        }
        return creaturesRegestry[ID];
    }

    public static bool CreatureExist(int ID){
        return creaturesRegestry.ContainsKey(ID);
    }

    public void OnLeftClick()
    {
        UnityEngine.Debug.Log("How did you get here?");
    }

    public void OnRightClick()
    {
        
    }

    public void OnSelect()
    {
        GameManager.CurrentSelected = this;
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        displayer.OnSelect();       
    }

    public void OnDeselect()
    {
        GameManager.CurrentSelected = null;
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        displayer.OnDeselect();       
    }

    public void InteractWithCreature(Creature creature){
        if(ability == null) {return;}
        if(creature.Player ==  Player){
            ability.ActionOnFriendlyCreature(creature);
        }
        else if(attacksPerTurn > _attacksAttempts) {
            _attacksAttempts++;
            ability.ActionOnEnemyCreature(creature);
        }
    }


    public void MoveTo(Vector3Int target){      
        int distance = WorldController.DistanceOf(position , target);
        if(distance <= _movement)
            {
                _movement -= distance;
                UpdatePosition(target);
            }
    }

    public override string ToString()
    {
        Card card = Card.GetCard(ID);
        if(card != null){
            return card.data.ToString();
        }
        return null;
    }

    void OnTurnStart(Turn turn){
        if(turn == null || turn.player == null){
            return;
        }
        if(turn.player == this.Player)
        {
            _movement = speed;
            _attacksAttempts = 0;
        }
    }

    public void UpdatePosition(Vector3Int newPosition)
    {
        //Clean old tile
        WorldTile tile = WorldController.Instance.world[position.x , position.y];
        tile.CreatureID = 0;
        
        //Set Creature ID to new tile
        tile = WorldController.Instance.world[newPosition.x , newPosition.y];
        tile.CreatureID = ID;

        var from = WorldController.Instance.map.CellToWorld(_position);
        _position = newPosition;

        //Move the displayer
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);
        var worldPosition = WorldController.Instance.map.CellToWorld(newPosition);
        displayer.transform.position = worldPosition;
        var res = WorldController.Instance.map.CellToWorld(_position);
        
    }
    public void ToastAttackFormated(int damage , int targetHitpoint)
    {
        CreatureDisplayer displayer = CreatureDisplayer.GetCreatureDisplayer(ID);

        if(damage <= 0){
            //Missed!
            displayer.Toast($"<color=grey><b><i>Missed! {damage}</color></b></I>" , 1);
            return;
        }

        float damageRelativeToHitpoints = damage / targetHitpoint;
        float fontSize = Mathf.Clamp( 35 * damageRelativeToHitpoints , 24 , 50);

        if(damageRelativeToHitpoints >= 1){
            //fatal hit
        displayer.Toast($"<color=red><b><i>FATAL! {damage}!</color></b></I>" , 1 , fontSize);
            return;
        }
            displayer.Toast($"<color=blue><b><i>{damage}</color></b></I>" , 1 ,fontSize);

        
    }

}
