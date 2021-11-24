using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Personality
{
    public List<Decision> decisions = new List<Decision>();
    public float mood = 0;

    public Personality(List<Decision> decisions)
    {
        this.decisions = decisions;
        decisions.Sort();
        decisions.Reverse();
    }

    public void Play(){
        if(mood < 0.1){
            mood += 0.1f;
            return;
        }

        if(mood >= 1){
            mood = 0;
            return;
        }

        Decision currentDecision = GetDecision(Random.Range(0 , decisions.Count - 1));
        currentDecision.Invoke();
    }

    private Decision GetDecision(int index , int iteration = 3)
    {
        if(iteration <= 0){
            return decisions[0];
        }
        Decision decision = decisions[index];
        if(decision == null){return GetDecision(index-1);}

        if(decision.severity >= mood){
            return decision;
        }
        return GetDecision(Random.Range(0 , index) , iteration - 1);
    }
}

