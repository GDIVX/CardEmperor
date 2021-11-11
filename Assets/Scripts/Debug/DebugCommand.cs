using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommand 
{
    public string name;
    Action command;

    public DebugCommand(String name,Action command){
        this.name = name;
        this.command  = command;
    }

    public void Invoke(){
        if(command == null){
            Debug.LogError("Can't find command");
            return;
        }
        command.Invoke();
    }
}
