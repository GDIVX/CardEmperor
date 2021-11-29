using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreatureInspector : MonoBehaviour
{
    private static CreatureInspector current;

    public GameObject GUI;
    
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

            current.GUI.transform.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.Attack,card.attack);
            current.GUI.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{creature.movement} / {GetFormatedString(creature.Speed,card.speed)}";

            current.GUI.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.Armor ,card.armor);
            current.GUI.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = GetFormatedString(creature.Hitpoint , card.hitpoint);

            //TODO remove
            current.GUI.transform.GetChild(4).gameObject.SetActive(false);
            current.GUI.transform.GetChild(5).gameObject.SetActive(false);
            current.GUI.transform.GetChild(6).gameObject.SetActive(false);

            current.GUI.gameObject.SetActive(true);
        });


    }

    static string GetFormatedString(int currentValue , int otherValue){


        if(currentValue > otherValue){
            return $"<color=green><b>{currentValue}</color></b>";
        }
        if(currentValue < otherValue){
            return $"<color=green><b>{currentValue}</color></b>";
        }
            return currentValue.ToString();
    }
}
