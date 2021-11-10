using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDFactory 
{
    private static int _Count = 0;

    public static int GetUniqueID(){
        _Count++;
        return _Count;
    }

    public static void ResetIDs(){
        _Count = 0;
    }
}
