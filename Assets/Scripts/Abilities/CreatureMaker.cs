﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMaker : CardAbility
{
    public int range;
    public bool pioneer = false;
    public bool amphibious = false;
    public bool flying = false;

    protected Creature creature;

    public override void Activate(Vector3Int targetPosition)
    {
        
        if(WorldController.Instance.IsTileExist(targetPosition)){
            WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];
            if((tile.feature == TileFeature.WATER && !amphibious) || tile.CreatureID != 0){
                return;
            }
            if(WorldController.Instance.IsInTerritory(targetPosition) == false && !pioneer){
                return;
            }
        }
        else{
            return;
        }

        if(!CardAbility.CanAfford(ID)){
            return;
        }

        Card parent = Card.GetCard(ID);
        var position = WorldController.Instance.map.GetCellCenterLocal(targetPosition);

        if(Creature.CreatureExist(ID)){
            Debug.LogError("Trying to create a creature that already was created");
            return;
        }

        creature = new Creature(parent.data.creatureData , ID , targetPosition);
        
        GameObject _gameObject = CreatureDisplayer.Create(creature , position);
        
        
        CreatureDisplayer displayer = _gameObject.GetComponent<CreatureDisplayer>();

        displayer.SetDisplay(true);

        CardsMannager.Instance.ExhaustCard(ID);
    }

    public override void Activate(CardDisplayer targetCard)
    {
        //nothing happenes 
    }

    protected override void OnStart()
    {
        
    }

    protected override void OnTriggerEnabled()
    {
        
    }
}