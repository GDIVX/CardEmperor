using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureAgent 
{
    public int ID => _ID;
    static Dictionary<int,CreatureAgent> agentsRegestry = new Dictionary<int, CreatureAgent>();

    protected int _ID;
    public Creature creature {get{return Creature.GetCreature(ID);}}

    public void SetID(int ID){
        _ID = ID;
        agentsRegestry.Add(_ID , this);
    }

    public static CreatureAgent GetAgent(int ID){
        return agentsRegestry[ID];
    }

    protected abstract float GetPositionScore(WorldTile tile , int depth);
    public abstract void OnTurnStart();

    public WorldTile GetFavorableTile(){
        Creature creature = Creature.GetCreature(ID);
        WorldTile currentTile = WorldController.Instance.world[creature.position.x , creature.position.y];
        WorldTile tile = SearchFavorablePosition(currentTile  , creature.speed);

        
        return tile;
    }

    protected WorldTile SearchFavorablePosition(WorldTile currentPosition, int searchRang){
        WorldTile[] tilesToSearch =currentPosition.GetTilesInRange(searchRang);

        float highestScore = 0;
        WorldTile res = currentPosition;

        foreach (WorldTile tile in tilesToSearch)
        {
            //Loop troughs each tile and score them. Each agent have diffrent score method
            float score = GetPositionScore(tile , 2);
            score = tile == currentPosition ? score/2 : score;

            if(score > highestScore){
                highestScore = score;
                res = tile;
            }
        }

        return res;
    }

}
