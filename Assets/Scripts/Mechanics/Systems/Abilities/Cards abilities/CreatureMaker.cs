﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class CreatureMaker : CardAbility
{
    public int range;
    public bool pioneer = false;
    public bool flying = false;

    protected Creature creature;

    protected override bool _Activate(Vector3Int targetPosition)
    {
        Card parent = Card.GetCard(ID);
        
        if(WorldController.Instance.IsTileExist(targetPosition)){
            WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];
            if( tile.CreatureID != 0){
                return false;
            }
            if(!tile.walkable && !parent.data.creatureData.flying){
                return false;
            }
        }
        else{
            return false;
        }

        var position = WorldController.Instance.map.GetCellCenterLocal(targetPosition);

        if(Creature.CreatureExist(ID)){
            Debug.LogError("Trying to create a creature that already was created");
            return false;
        }

        creature = new Creature(parent.data.creatureData , ID , targetPosition);
        
        GameObject _gameObject = CreatureDisplayer.Create(creature , position);
        
        
        CreatureDisplayer displayer = _gameObject.GetComponent<CreatureDisplayer>();

        displayer.SetDisplay(true);

        if(creature.PlayerID == Player.Main.ID){
            CardDisplayer.GetDisplayer(ID).OnDeselect();
            CardsMannager.Instance.hand.RemoveCard(ID);
        }
            

        return true;
    }

    protected override void OnStart()
    {
        
    }

    public override bool isPlayableOnTile(WorldTile tile)
    {
        CreatureData data = Card.GetCard(ID).data.creatureData;
        return tile.CreatureID == 0 && (data.flying || tile.walkable);
    }
}
