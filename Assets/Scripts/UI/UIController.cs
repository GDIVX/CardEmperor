using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance =>_instance;
    static UIController _instance;

    public TextMeshProUGUI food,ind,magic, know;

    public HandGUI handGUI;

    void Awake()
    {
        if(instance == null){
            _instance = this;
        }

        handGUI = GameObject.FindObjectOfType<HandGUI>();
        if(handGUI == null){Debug.LogError("can't find HandGUI script");}
    }

    void Start()
    {
        //Register to delegates
        Player.Main.foodPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.industryPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.magicPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.knowledge.RegisterOnValueChange(OnManaValueChange);
    }

    public void NextTurnBtn(){
        GameManager.Instance.EndTurnButton();
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
