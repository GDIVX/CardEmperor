using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class WorldTile : IClickable 
{
    public Vector2Int position{get => _position;}
    public TileFeature feature{get=>_feature; set => SetFeature(value);}
    [ShowInInspector]
    [ReadOnly]
    public int CreatureID = 0;

    [ShowInInspector]
    [ReadOnly]
    private Vector2Int _position;
    [ShowInInspector]
    [ReadOnly]
    private TileFeature _feature;

    public WorldTile(Vector2Int position , TileFeature feature){
        _position = position;
        SetFeature(feature);
    }
    public WorldTile[] GetNeighbors()
    {
        return GetTilesInRange(1);
    }


    public WorldTile[] GetTilesInRange(int range){

        List<WorldTile> res = new List<WorldTile>();
        //range++;
        Vector3Int cubePosition = WorldController.CordsToCube((Vector3Int)position);
        
        for (int x = -range; x <= range; x++)
        {
            int  start = Mathf.Max(-range , -x - range);
            int end = Mathf.Min(range , -x + range);

            for (var y = start; y <= end; y++)
            {
                int z = -x-y;
                Vector3Int sampledPosition = WorldController.CubeToCords(new Vector3Int(x,y,z));
                sampledPosition += (Vector3Int)position;
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
        res[0] = definition.Food;
        res[1] = definition.Industry;
        res[2] = definition.Magic;
        res[3] = definition.Knowledge;

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
    WATER , PLAINS, MARSH , MOUNTIAN , PEAK 
    ,FOREST, IRON , FISH, LAYLINE , FIELD 

}
