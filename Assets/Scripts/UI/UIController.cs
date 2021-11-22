using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Mechanics.Systems.Players;

public class UIController : MonoBehaviour
{
    public static UIController Instance =>_instance;
    static UIController _instance;

    public TextMeshProUGUI food,ind,magic, know;

    public HandGUI handGUI;
    public ClockUI clockUI;
    public EventWindow eventWindow;


    void Awake()
    {
        if(Instance == null){
            _instance = this;
        }
    }

    void Start()
    {
        //Register to delegates
        Player.Main.foodPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.industryPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.magicPoints.RegisterOnValueChange(OnManaValueChange);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            handGUI.ScaleUp();
            return;
        }
        if(Input.GetKeyDown(KeyCode.S)){
            handGUI.ScaleDown();
            return;
        }
        
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
        }
    }

        public static int GetDisabledChildrenCount(Transform t){
        int res = 0;
        for (var i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            res = child.gameObject.activeInHierarchy ? res : res+1; 
        }
        return res;
    }

    public static GameObject GetDisabledChild(Transform t){
        for (var i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            if(child.gameObject.activeInHierarchy == false){
                return child.gameObject;
            }
        }
        return null;
    }
}
