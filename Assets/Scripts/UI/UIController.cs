using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Mechanics.Systems.Players;

public class UIController : MonoBehaviour
{
    public static UIController Instance =>_instance;
    static UIController _instance;

    public TextMeshProUGUI food,ind,magic, drawPileTxt ,discardPileTxt, exilePileTxt;

    public HandGUI handGUI;
    public EventWindow eventWindow;



    void Awake()
    {
        if(Instance == null){
            _instance = this;
        }

    }

    public void Init()
    {
        //Register to delegates
        Player.Main.foodPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.industryPoints.RegisterOnValueChange(OnManaValueChange);
        Player.Main.magicPoints.RegisterOnValueChange(OnManaValueChange);

        CardsMannager.Instance.drawPile.OnValueChange += OnPileValueChanged;
        CardsMannager.Instance.discardPile.OnValueChange += OnPileValueChanged;
        CardsMannager.Instance.exilePile.OnValueChange += OnPileValueChanged;
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

    void OnPileValueChanged(Pile pile){
        switch(pile.pileType){
            case Pile.PileType.Draw:
                drawPileTxt.text = pile.Size.ToString();
                break;
            case Pile.PileType.Discard:
                discardPileTxt.text = pile.Size.ToString();
                break;
            case Pile.PileType.Exile:
                exilePileTxt.text = pile.Size.ToString();
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
    public static GameObject[] GetDisableChildren(Transform t){
        List<GameObject> res = new List<GameObject>();
        
        foreach (Transform child in t)
        {
            if(child.gameObject.activeInHierarchy == false) 
                res.Add(child.gameObject);
        }

        return res.ToArray();
    }
}
