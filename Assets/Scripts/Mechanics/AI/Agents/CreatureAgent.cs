using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Mechanics.AI;
using UnityEngine;

[System.Serializable]
public abstract class CreatureAgent 
{
    public State state;
    public string debug_currentState;
    public int ID => _ID;
    static Dictionary<int,CreatureAgent> agentsRegestry = new Dictionary<int, CreatureAgent>();

    protected int _ID;
    public Creature creature {get{return Creature.GetCreature(ID);}}

    public Stack<State> states = new Stack<State>();

    public void SetID(int ID){
        _ID = ID;
        agentsRegestry.Add(_ID , this);
    }

    public static CreatureAgent GetAgent(int ID){
        return agentsRegestry[ID];
    }

    public static bool AgentExist(int ID){
        return agentsRegestry.ContainsKey(ID);
    }

    public abstract State GetNextState(WorldTile tile);

    protected abstract float GetPositionScore(WorldTile tile , int depth);
    public void OnTurnStart()
        {

            WorldTile tile = WorldController.Instance.world[creature.position.x , creature.position.y];
            states.Push(GetNextState(tile));
            
            state = states.Pop();
            state.Activate(this);
        }
    
    public void OnStateActivatedDone(){
        WorldTile tile = WorldController.Instance.world[creature.position.x , creature.position.y];
        State newState = GetNextState(tile);

        if(newState != null){
            states.Push(newState);
        }

        if(states.Count > 0){
            state = states.Pop();
            state.Activate(this);
        }
    }

    public WorldTile GetFavorableTile(WorldTile[] tilesToSearch){
        Creature creature = Creature.GetCreature(ID);
        WorldTile currentTile = WorldController.Instance.world[creature.position.x , creature.position.y];
        WorldTile tile = SearchFavorablePosition(currentTile  , tilesToSearch);
        
        return tile;
    }

    protected WorldTile SearchFavorablePosition(WorldTile currentPosition, WorldTile[] tilesToSearch){

        float highestScore = 0;
        WorldTile res = currentPosition;

        foreach (WorldTile tile in tilesToSearch)
        {
            //Loop troughs each tile and score them. Each agent have diffrent score method
            float score = GetPositionScore(tile , 2);
            //score = (tile == currentPosition) ? score/2 : score;

            if(score > highestScore){
                highestScore = score;
                res = tile;
            }
            else if(score == highestScore){
                //pick one at random
                float rand = Random.value;
                if(rand >= .5f){
                    res = tile;
                }
            }
        }

        return res;
    }

}
