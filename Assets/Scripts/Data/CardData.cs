using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Sirenix.OdinInspector;
using static RandomSelector;

[PreviewField]
[CreateAssetMenu(menuName = "Cards/Deck Data")]
public class CardData : ScriptableObject
{
    [HorizontalGroup("Data", 75)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite image;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    public string cardName;
    public CardType cardType;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    [TextArea]
    public string description;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    public KeywordDefinition[] keywords;
    [BoxGroup("Script")]
    [HideLabel]
    [LabelWidth(100)]
    public string abilityScriptName;
    [BoxGroup("Mana Cost")]
    [GUIColor(0, 1, 0)]
    [LabelWidth(100)]
    [Range(0, 50)]
    public int foodPrice;
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0, 50)]
    [GUIColor(1, 0, 0)]
    public int industryPrice;
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0, 50)]
    [GUIColor(0, 0, 1)]
    public int MagicPrice;
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0, 50)]
    [GUIColor(1, 0, 1)]
    public int Time;
    [Range(0, 1)]
    public float priority;
    public CreatureData creatureData;
    public Rarity rarity;
    public int parm1, parm2, parm3;
    public string stringParm;

    public enum CardType { Creature, Town, Fort, worker, Magic, Miracle, Fate, Curse }

    public List<CardData> UpgradeOptions, UnlockCards;

    public bool Exile;
}

[System.Serializable]
public class CreatureData
{
    [HorizontalGroup("Data", 75)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite icon;
    [VerticalGroup("Data/Stats")]
    [LabelWidth(100)]
    public int hitpoint, speed, armor, attack, attackRange, attacksPerTurn;
    [BoxGroup("script")]
    [HideLabel]
    [LabelWidth(100)]
    [HideInInlineEditors]
    public string abilityScriptName;
    [BoxGroup("Properties")]
    [LabelWidth(100)]
    public bool amphibious, flying, pioneer;
}



