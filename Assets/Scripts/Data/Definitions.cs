using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game Data/Definitions")]
public class Definitions : ScriptableObject
{
    [SerializeField]
    [TabGroup("Features")]
    List<FeatureDefinition> _FeatureDefinitions;
    Dictionary<TileFeature, FeatureDefinition> featuresRegestry = new Dictionary<TileFeature, FeatureDefinition>();

    [SerializeField]
    [TabGroup("Keywords")]
    List<KeywordDescription> _keywordDescriptions;
    Dictionary<KeywordDefinition.Keyword, string> keywordsTooltipTexts = new Dictionary<KeywordDefinition.Keyword, string>();
    public void Start()
    {
        foreach (var feature in _FeatureDefinitions)
        {
            featuresRegestry.Add(feature.feature, feature);
        }
        foreach (var desc in _keywordDescriptions)
        {
            keywordsTooltipTexts.Add(desc.keyword, desc.description);
        }
    }

    public FeatureDefinition GetFeatureDefinition(TileFeature feature)
    {
        return featuresRegestry[feature];
    }

    public string GetKeywordTooltipText(KeywordDefinition definition)
    {
        if (keywordsTooltipTexts.ContainsKey(definition.keyword) == false)
        {
            Debug.LogError($"Keyword {definition.keyword} is not defined. Please check Resources/Data/Definition");
            return null;
        }
        string text = keywordsTooltipTexts[definition.keyword];
        string value = definition.value.ToString();
        text = text.Replace("_X_", value);
        return text;
    }

}

[System.Serializable]
public class FeatureDefinition
{
    [BoxGroup("Feature")]
    public TileFeature feature;

    [BoxGroup("Mana")]
    public float Food, Industry, Magic, Knowledge;
}

[System.Serializable]
internal class KeywordDescription
{
    public KeywordDefinition.Keyword keyword;
    public string description;
}
[System.Serializable]
public class KeywordDefinition
{
    [Range(0, 10)]
    public int value;
    public Keyword keyword;
    public enum Keyword
    {
        Range,
        Strength,
        Vigor,
        Formation,
        Aim,
        Omen,
        Toughness,
        Exile,
        Build,
        Spikes,
        Upgrade
    }
}
