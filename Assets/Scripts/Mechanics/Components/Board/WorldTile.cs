using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class WorldTile : IClickable 
{
    public Vector2Int position{get => _position;}
    public Vector3Int CubePosition{get => _cubePosition;}
    public TileFeature feature{get=>_feature; set => SetFeature(value);}
    [ShowInInspector]
    [ReadOnly]
    public int CreatureID = 0;
    public bool walkable;

    [ShowInInspector]
    [ReadOnly]
    private Vector2Int _position;
    [ShowInInspector]
    [ReadOnly]
    private TileFeature _feature;
    [ShowInInspector]
    [ReadOnly]
    Vector3Int _cubePosition;

    public WorldTile(Vector2Int position , TileFeature feature , bool walkable = true){
        _position = position;
        _cubePosition = WorldController.CordsToCube((Vector3Int)position);
        this.walkable = walkable;
        SetFeature(feature);


    }
    public WorldTile[] GetNeighbors()
    {
        List<WorldTile> res = new List<WorldTile>(GetTilesInRange(1));
        res.Remove(this);
        return res.ToArray();
    }


    public WorldTile[] GetTilesInRange(int range){

        List<WorldTile> res = new List<WorldTile>();
        //range++;
        
        for (int x = -range; x <= range; x++)
        {
            int  start = Mathf.Max(-range , -x - range);
            int end = Mathf.Min(range , -x + range);

            for (var y = start; y <= end; y++)
            {
                int z = -x-y;

                Vector3Int sampledPosition = WorldController.CubeToCords(new Vector3Int(x,y,z));
                sampledPosition = WorldController.CubeToCords(sampledPosition + CubePosition);
                
                if(WorldController.Instance.IsTileExist(sampledPosition)){
                    WorldTile tile = WorldController.Instance.world[sampledPosition.x , sampledPosition.y];
                    res.Add(tile);
                }
            }
            
        }

        return res.ToArray();
    }

    private void SetFeature(TileFeature feature)
    {
        _feature = feature;
    }

    internal int[] GetIncome()
    {
        var definition = GameManager.Instance.definitions.GetFeatureDefinition(feature);
        int[] res = new int[4];
        res[0] = Mathf.FloorToInt(definition.Food);
        res[1] = Mathf.FloorToInt(definition.Industry);
        res[2] = Mathf.FloorToInt(definition.Magic);
        res[3] = Mathf.FloorToInt(definition.Knowledge);

        return res;
    }
    public void OnLeftClick()
    {
        IClickable CurrentSelectedID = GameManager.CurrentSelected;
        if((object)CurrentSelectedID != this && CurrentSelectedID != null){
            //something else is selected
            //clear selection
            CurrentSelectedID.OnDeselect();
        }

        //Select this displayer
        OnSelect();
    }

    public void OnRightClick()
    {
        IClickable CurrentSelected = GameManager.CurrentSelected;

        if(CurrentSelected == null){
            return;
        }
        else if(CurrentSelected.GetType() == typeof(CardDisplayer)){
            CardDisplayer displayer = (CardDisplayer)CurrentSelected;
            CardAbility ability = CardAbility.GetAbility(displayer.ID);
            ability.Activate((Vector3Int)_position);
        }
        else if(CurrentSelected.GetType() == typeof(CreatureDisplayer)){
            CreatureDisplayer displayer = (CreatureDisplayer)CurrentSelected;
            int selectedCreatureID = displayer.ID;
            Creature selectedCreature = Creature.GetCreature(selectedCreatureID);

            if(selectedCreature == null){
                Debug.LogError($"Can't find creature with the ID {selectedCreatureID}");
                return;
            }
            if(selectedCreature.Player.IsMain()){
                if(feature != TileFeature.WATER || selectedCreature.amphibious ){
                    if(CreatureID == 0)
                        selectedCreature.FlyTo((Vector3Int)position);
                    else
                        selectedCreature.InteractWithCreature(Creature.GetCreature(CreatureID));
                }
            }
        }
    }

    public void OnSelect()
    {
        if(CreatureID != 0){
            Creature.GetCreature(CreatureID).OnSelect();
        }
    }

    public void OnDeselect()
    {
        if(CreatureID != 0){
            Creature.GetCreature(CreatureID).OnDeselect();
        }
        
    }
}

[System.Serializable]
public enum TileFeature{
    WATER , PLAINS,MOUNTIAN  
    ,FOREST, FOREST_HEART , FARM, FIELD , ORB_HEART , ORBS

}
