using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase 
{
    string _commandID;
    string _commandDescription;
    string _commandFormat;

    public string commandID => _commandID;
    public string commandDescription => commandDescription;
    public string commandFormat => _commandFormat;

    public DebugCommandBase(string id, string description , string format){
        _commandID = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    Action command;
    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(){
        command.Invoke();
    }
}

public class DebugCommand<T> : DebugCommandBase{
    Action<T> command;

    public DebugCommand(string id, string description, string format, Action<T> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T value){
        command.Invoke(value);
    }
}