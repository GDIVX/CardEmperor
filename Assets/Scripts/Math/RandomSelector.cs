using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RandomSelector : MonoBehaviour
{
    [OnValueChanged("calculateValues")]
    [Range(0,1)]
    public float commonRarity = .80f;
    [OnValueChanged("calculateValues")]
    [Range(0,1)]
    public float uncommonRarity = .20f;
    [OnValueChanged("calculateValues")]
    [Range(-1,1)]
    public float rareRarity;

    float saveCommon = 0;


    void Start()
    {
        calculateValues();
    }
    void calculateValues(){
        rareRarity = 1 - (commonRarity + uncommonRarity);
        if(saveCommon == 0) saveCommon = commonRarity;
    }

    public enum Rarity{ COMMON , UNCOMMON , RARE}

    Rarity GetRandomRarity(){
        float rand = Random.value;
        if(rand <= rareRarity) return Rarity.RARE;
        if(rand <= uncommonRarity) return Rarity.UNCOMMON;
        return Rarity.COMMON;
    }

    public Rarity GetRarity(){
        Rarity res = GetRandomRarity();
        if(res == Rarity.RARE){
            commonRarity = saveCommon;
        }
        else if(commonRarity > 0.10f){
            commonRarity -= .05f;
        }
        else{
            uncommonRarity -=0.05f;
        }
        calculateValues();
        return res;
    } 

    [Button]
    void test(){
        Debug.Log(GetRarity());
    }
}
