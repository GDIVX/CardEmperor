using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game Data/Definitions")]
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
    [BoxGroup("Feature")]
    public TileFeature feature;
    [BoxGroup("Feature")]
    public TileFeature improveTo;
    [BoxGroup("Mana")]
    public float Food , Industry , Magic, Knowledge;
}
