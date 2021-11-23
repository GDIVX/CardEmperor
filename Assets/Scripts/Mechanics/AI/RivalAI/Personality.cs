using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

        Decision currentDecision = GetDecision();
        currentDecision.Invoke();
    }

    private Decision GetDecision()
    {
        foreach (Decision d in decisions)
        {
            if(d.severity >= mood) return d;
        }

        return decisions[decisions.Count-1];
    }
}

