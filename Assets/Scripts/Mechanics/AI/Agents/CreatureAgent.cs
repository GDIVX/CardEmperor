using System;
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

    internal bool IsTargetDeadOrNull()
    {
        return target == null || target.isAlive == false;
    }

    protected int _ID;
    public Creature creature {get{return Creature.GetCreature(ID);}}

    internal void SetTarget(Creature creature)
    {
        target = creature;
    }

    public Creature target;

    public void SetID(int ID){
        _ID = ID;
        agentsRegestry.Add(_ID , this);
    }

    public CreatureAgent(){
        state = new IdleState();
    }

    public static CreatureAgent GetAgent(int ID){
        if(agentsRegestry.ContainsKey(ID))
            return agentsRegestry[ID];
        return null;
    }

    public static bool AgentExist(int ID){
        return agentsRegestry.ContainsKey(ID);
    }

    public void OnTurnStart()
    {
        Debug.Log($"attacks: {creature.attacksAttempts} movement: {creature.movement}");
        State nextState = state?.Activate(this);
        
        if(nextState != null){
            state = nextState;
        }
        else{
            state = new IdleState().Activate(this);
        }
    }
}
