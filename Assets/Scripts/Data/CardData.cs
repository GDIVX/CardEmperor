using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CardData : ScriptableObject 
{
   public int foodPrice , industryPrice , MagicPrice ;
   public string cardName, keyword , description , abilityScriptName;
   public Sprite image;
   [Range(0,1)]
   public float priority;
   public CreatureData creatureData;
}

[System.Serializable]
public class CreatureData
{
   public int hitpoint, speed, armor, attack , attackRange;
   public Sprite icon;
   public bool amphibious , flying , pioneer;
   public string abilityScriptName;
}

