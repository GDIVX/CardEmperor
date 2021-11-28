using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using static RandomSelector;

public class CardMaker : OdinEditorWindow
{
    [MenuItem("Cards/Card Maker")]
    private static void OpenWindow(){
        GetWindow<CardMaker>().Show();
    }

    [ShowInInspector]
    [EnumToggleButtons]
    [HideLabel]
    CardData.CardType cardType;

    [HorizontalGroup("Data" , 75)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite image;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    [InfoBox("Must provide a valid name" ,InfoMessageType.Error , "NotValidName" )]
    public string cardName;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    [TextArea]
    public string description;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    public Rarity rarity;
    [VerticalGroup("Data/Info")]
    [LabelWidth(100)]
    public KeywordDefinition[] keywords;
    [VerticalGroup("Data/Associated Cards")]
    [LabelWidth(100)]
    public List<CardData> UpgradeOptions , UnlockCards;
    [BoxGroup("Script")]
    [HideLabel]
    [LabelWidth(100)]
    [Sirenix.OdinInspector.FilePath]
    public string abilityScript = "Assets/Scripts/Mechanics/Systems/Abilities/Cards abilities";
    [HideIf("cardType" , CardData.CardType.Fate)]
    [BoxGroup("Mana Cost")]
    [GUIColor(0,1,0)]
    [LabelWidth(100)]
    [Range(0,50)]
    public int foodPrice;
    [HideIf("cardType" , CardData.CardType.Fate)]
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0,50)]
    [GUIColor(1,0,0)]
    public int industryPrice;
    [HideIf("cardType" , CardData.CardType.Fate)]
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0,50)]
    [GUIColor(0,0,1)]
    public int MagicPrice ;
    [ShowIf("cardType" , CardData.CardType.Fate)]
    [BoxGroup("Mana Cost")]
    [LabelWidth(100)]
    [Range(0,50)]
    [GUIColor(.8f,.4f,0)]
    public int Time ;
    [Range(0,1)]
    public float priority;
    [BoxGroup("Creature")]
    [ShowIf("ToggleShowCreatureData")]
    public CreatureData creatureData;

    [BoxGroup("Creature")]
    [HideLabel]
    [LabelWidth(100)]
    [ShowIf("ToggleShowCreatureData")]
    [Sirenix.OdinInspector.FilePath]
    public string creatureAbility = "Assets/Scripts/Mechanics/Systems/Abilities/Creature Abilities";
    //TODO create icon for creatures procedurally


    [HideLabel]
    [InlineProperty()]
    [FolderPath]
    [InfoBox("Must provide a valid folder path" ,InfoMessageType.Error , "NotValidPath" )]
    public string SaveFolder = "Assets/Resources/Data/Cards";
    [Button("Save")]
    public void CreateCard(){
        CardData data = ScriptableObject.CreateInstance<CardData>();

        data.cardType = cardType;
        data.image = image;
        data.cardName = cardName;
        data.description = description;
        data.keywords = keywords;
        char[] charsToTrim = { 'c', ' ', '\'' , 's' , '.'};
        data.abilityScriptName = Path.GetFileName(abilityScript).Trim(charsToTrim);
        data.foodPrice = foodPrice;
        data.industryPrice = industryPrice;
        data.MagicPrice = MagicPrice;
        data.Time = Time;
        data.priority = priority;
        creatureData.abilityScriptName = Path.GetFileName(creatureAbility).Trim(charsToTrim);
        data.creatureData = creatureData;
        data.UpgradeOptions = UpgradeOptions;
        data.UnlockCards = UnlockCards;
        data.rarity = rarity;

        AssetDatabase.CreateAsset(data , $"{SaveFolder}/{cardName}.asset");
        AssetDatabase.SaveAssets();
    }

    bool ToggleShowCreatureData(){
        return (cardType == CardData.CardType.Creature ||
        cardType == CardData.CardType.Town ||
        cardType == CardData.CardType.Fort ||
        cardType == CardData.CardType.worker);
    }

    bool NotValidName(){
        return (cardName == null || cardName == "");
    }
    bool NotValidPath(){
        return !AssetDatabase.IsValidFolder(SaveFolder);
    }


    public enum CardType{Creature , Town , Fort , outpost , Magic , Miracle , Fate , Curse}
}
