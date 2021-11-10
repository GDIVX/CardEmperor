using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Player Player{get{return Player.GetPlayer(PlayerID);}}
    public bool amphibious{get{return _amphibious;}}
    public bool flying{get{return _flying;}}
    public bool pioneer{get{return _pioneer;}}


    static Dictionary<int,Creature> creaturesRegestry = new Dictionary<int, Creature>();

    int hitpoint, armor, attack , speed, attackRange , _ID , _PlayerID , movement;
    Sprite _icon;

    Vector3Int _position;
    private bool _amphibious , _flying , _pioneer;

    public Creature(CreatureData data , int ID , Vector3Int position){
        this.hitpoint = data.hitpoint;
        this.armor = data.armor;
        this.attack = data.attack;
        this.speed = data.speed;
        this.attackRange = data.attackRange;
        this._icon = data.icon;
        _position = position;
        movement = speed;

        _amphibious = data.amphibious;
        _flying = data.flying;
        _pioneer = data.pioneer;

        _ID = ID;
        Card card = Card.GetCard(ID);
        _PlayerID = card.playerID;
        creaturesRegestry.Add(ID , this);
        WorldController.Instance.world[position.x , position.y].CreatureID = ID;


        GameManager.Instance.RegisterToTurnStart(OnTurnStart);
    }

    public static Creature GetCreature(int ID){
        if(CreatureExist(ID) == false){
            Debug.LogError("Trying to get a creature that dose not exist");
            return null;
        }
        return creaturesRegestry[ID];
    }

    public static bool CreatureExist(int ID){
        return creaturesRegestry.ContainsKey(ID);
    }

    public void OnLeftClick()
    {
        Debug.Log("How did you get here?");
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

    public override string ToString()
    {
        Card card = Card.GetCard(ID);
        if(card != null){
            return card.data.ToString();
        }
        return null;
    }

    public void MoveTo(Vector3Int target){
        //TODO placeholder, replace with a pathfinding algorithm 
        
        FlyTo(target);
    }
    public void FlyTo(Vector3Int target){      
        int distance = WorldController.DistanceOf(position , target);
        if(distance <= movement)
            {
                movement -= distance;
                UpdatePosition(target);
            }
    }

    void OnTurnStart(Player player){
        if(player == this.Player)
        {
            movement = speed;
        }
    }

    private void UpdatePosition(Vector3Int newPosition)
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
}
