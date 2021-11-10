using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card game/Definitions")]
public class Definitions : ScriptableObject
{
    [SerializeField]
    List<FeatureDefinition> _FeatureDefinitions;
    Dictionary<TileFeature , FeatureDefinition> featuresRegestry = new Dictionary<TileFeature, FeatureDefinition>();

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
}

[System.Serializable]
public class FeatureDefinition{
    public TileFeature feature;
    public TileFeature improveTo;
    public int Food , Industry , Magic, Knowledge;
}
