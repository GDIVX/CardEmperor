using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision : IComparer<Decision>
{
    public float severity;

    Action action;

    public Decision(float severity, Action action)
    {
        this.severity = severity;
        this.action = action;
    }

    public int Compare(Decision x, Decision y)
    {
        return Mathf.RoundToInt(x.severity - y.severity);
    }

    public void Invoke(){
        action?.Invoke();
    }
}
