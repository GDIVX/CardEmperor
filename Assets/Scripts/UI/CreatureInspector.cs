using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Mechanics.Components.Effects;

public class CreatureInspector : MonoBehaviour
{
    private static CreatureInspector current;

    public GameObject GUI;
    public GameObject effectGUI;
    
    int ID = 0;

    void Awake()
    {
        if(current == null){
            current = this;
        }
        else{
            GameObject.Destroy(this);
        }
            GUI = GameObject.Instantiate(GUI);
            GUI.transform.SetParent(transform);
            GUI.transform.position = transform.position;
            GUI.SetActive(false);
    }

    public static void ShowCreatureCard(int ID){
        if(ID == 0){
            return;
        }
        current.SetID(ID);
        Show();
    }

    public static void Hide(){
        CreatureDisplayer displayer = GameManager.CurrentSelected as CreatureDisplayer;
        if(displayer != null){
            ShowCreatureCard(displayer.ID);
        }
        else{
            current.GUI.gameObject.SetActive(false);
        }
    }

    void SetID(int ID){
        this.ID = ID;
    }

    static void Show(){
        if(current.ID == 0){
            Debug.LogError("ID can't be 0");
            return;
        }
        if(!Creature.CreatureExist(current.ID)){
            Debug.LogError("Can't find creature");
        }

        Card card = Card.GetCard(current.ID);
        Creature creature = Creature.GetCreature(current.ID);

        var delay = LeanTween.delayedCall(.2f , ()=>{

            current.GUI.transform.GetChild(2).GetChild(0).
                GetComponent<TextMeshProUGUI>().text = card.data.cardType.ToString();

            current.GUI.transform.GetChild(0).GetComponent<Image>().sprite = card.data.image;

            current.GUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.cardName;

            current.GUI.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.description;
            current.GUI.transform.GetChild(3).GetComponent<TooltipTrigger>().SetTextFromCard(card.data.keywords);

            current.GUI.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.hitpoints , card.hitpoint);
            current.GUI.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.armor ,card.armor);
            current.GUI.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{creature.movement} / {GetFormatedString(creature.speed,card.speed)}";
            current.GUI.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.attack,card.attack);
            
            if(GameManager.Instance.DebugMode){
                Creature selectedCreature = Creature.GetCreature(current.ID);
                if(selectedCreature != null){
                    current.GUI.transform.Find("Debug State").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                        CreatureAgent.GetAgent(current.ID) != null ? CreatureAgent.GetAgent(current.ID).state.ToString() : "";
                    
                    current.GUI.transform.Find("Debug ID").GetChild(0).GetComponent<TextMeshProUGUI>().text = current.ID.ToString();
                    current.GUI.transform.Find("Debug Position").GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedCreature.position.ToString();
                }
            }
            else{
                current.GUI.transform.Find("Debug State").gameObject.SetActive(false);
                current.GUI.transform.Find("Debug ID").gameObject.SetActive(false);
                current.GUI.transform.Find("Debug Position").gameObject.SetActive(false);
            }

            ShowEffects();

            current.GUI.gameObject.SetActive(true);
        });
    }

    static void ShowEffects(){
        Card card = Card.GetCard(current.ID);
        Creature creature = Creature.GetCreature(current.ID);
        Transform transform = current.GUI.transform.GetChild(8);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        var effects = creature.effects;
        foreach (var typeEffectPair in effects)
        {
                GameObject child = null;
            if(UIController.GetDisabledChildrenCount(transform) > 0){
                child  = UIController.GetDisabledChild(transform);
            }
            else{
                child = GameObject.Instantiate(current.effectGUI);
                child.transform.SetParent(transform);
            }
            Effect effect = typeEffectPair.Value;

            child.GetComponent<Image>().color = effect.UIData.color;

            TooltipTrigger tooltip = child.GetComponent<TooltipTrigger>();
            tooltip.headers.Clear();
            tooltip.contents.Clear();
            tooltip.anchor = current.GUI.transform.GetChild(3).GetChild(1).gameObject.GetComponent<RectTransform>();
            tooltip.headers.Add(GetFormatedEffectTooltip(effect.UIData.toolTipHeader , effect.value));
            tooltip.contents.Add(GetFormatedEffectTooltip(effect.UIData.tooltipDescription , effect.value));

            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = effect.value.ToString();
            child.transform.GetChild(1).GetComponent<Image>().sprite = effect.UIData.icon;

            child.SetActive(true);
        }
    }

    static string GetFormatedEffectTooltip(string original , int value){
        return original.Replace("_X_" , value.ToString());
    }

    static string GetFormatedString(int currentValue , int otherValue){


        if(currentValue > otherValue){
            return $"<color=green><b>{currentValue}</color></b>";
        }
        if(currentValue < otherValue){
            return $"<color=red><b>{currentValue}</color></b>";
        }
            return currentValue.ToString();
    }
}
