using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game Data/Definitions")]
public class Definitions : ScriptableObject
{
    [ShowInInspector]
    [TabGroup("Features")]
    List<FeatureDefinition> _FeatureDefinitions;
    [TabGroup("Features")]
    Dictionary<TileFeature , FeatureDefinition> featuresRegestry = new Dictionary<TileFeature, FeatureDefinition>();

    [TabGroup("Keywords")]
    [ShowInInspector]
    Dictionary<KeywordDefinition.Keyword,string> keywordsTooltipTexts = new Dictionary<KeywordDefinition.Keyword, string>();
    public void Start()
    {
        foreach (var feature in _FeatureDefinitions)
        {
            featuresRegestry.Add(feature.feature , feature);
        }
    }

    public FeatureDefinition GetFeatureDefinition(TileFeature feature){
        return featuresRegestry[feature];
    }

    public string GetKeywordTooltipText(KeywordDefinition definition){
        if(keywordsTooltipTexts.ContainsKey(definition.keyword) == false){
            Debug.LogError($"Keyword {definition.keyword} is not defined. Please check Resources/Data/Definition");
            return null;
        }
        string text = keywordsTooltipTexts[definition.keyword];
        string value = definition.value.ToString();
        text = text.Replace("_X_",value);
        return text;
    }

}

[System.Serializable]
public class FeatureDefinition{
    [BoxGroup("Feature")]
    public TileFeature feature;
    [BoxGroup("Feature")]
    public TileFeature improveTo;
    [BoxGroup("Mana")]
    public float Food , Industry , Magic, Knowledge;
}

[System.Serializable]
public class KeywordDefinition
{
    [Range(0,10)]
    public int value;
    public Keyword keyword;
    public enum Keyword
    {
        Range,
        Strength,
        Vigor,
        Heal,
        Flexible,
        Haste,
        Weakness,
        Vulnerable,
        Pioneer,
        Flying,
        Amphibious,
        Teleport,
        Charge,
        Dualist,
        Piercing,
        Sharp,
        Shred,
        Fortify,
        Spellcaster,
        Battle_Caster,
        Miracle,
        Decree,
        Creature,Town,Outpost,Fort,Spell,
        Exhuast,
        Revive,
        Bless,
        Protector,
        Charity,
        Divine,
        Aim
    }
}
