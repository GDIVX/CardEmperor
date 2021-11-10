using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI food,ind,magic, know;


    void Start()
    {
        //Register to delegates
        Player.Main.foodPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.industryPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.magicPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.knowledge.RegisterOnValueChange(OnManaValueChange);
    }

    void OnManaValueChange(int value , ManaType manaType){
        switch(manaType){
            case ManaType.FOOD:
                food.text = value.ToString();
                break;
            case ManaType.INDUSTRY:
                ind.text = value.ToString();
                break;
            case ManaType.MAGIC:
                magic.text = value.ToString();
                break;
            case ManaType.KNOWLEDGE:
                know.text = value.ToString();
                break;
        }
    }
}
