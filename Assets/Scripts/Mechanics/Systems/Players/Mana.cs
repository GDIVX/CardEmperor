using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana 
{
    public int value;
    public int income;
    public ManaType manaType;

    public Mana(ManaType manaType){
        this.manaType = manaType;
    }

    Action<int , ManaType> valueChangedDelegate;


    public void RegisterOnValueChange(Action<int , ManaType> action){
        valueChangedDelegate += action;
    }

    public void SetValue(int value){
        valueChangedDelegate?.Invoke(value , manaType);
        this.value = value;
    }
}



public enum ManaType{
    FOOD , INDUSTRY , MAGIC, KNOWLEDGE
}
