using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayer : MonoBehaviour , IClickable
{
    public Action OnUpdateDisplayerListener;
    public int ID{get{return _ID;}}

    private int _ID;

    private static Dictionary<int, CardDisplayer> CardDisplayerRegestry = new Dictionary<int, CardDisplayer>();

    public static GameObject Create(){
        
        GameObject displayGameObject = CardsMannager.Instance.cardTamplate;
        CardDisplayer displayer = displayGameObject.GetComponent<CardDisplayer>();

        return displayGameObject;
    }

    public void SetID(int ID){
        this._ID = ID;
        CardDisplayer.CardDisplayerRegestry[ID] = this;
    }

    public void Clear(){
        CardDisplayer.CardDisplayerRegestry[_ID] = null;
        _ID = 0;
    }


    public void SetDisplayActive(bool isActive){
        if(isActive){
            ShowDisplay();
        }
        else{
            gameObject.SetActive(false);
        }
    }


    private void ShowDisplay()
    {

        if(ID == 0){
            Debug.LogError("Did not set ID for displayer");
            return;
        }

        Card card = Card.GetCard(ID);
        
        if(card == null){
            Debug.LogError("card is not set to displayer");
            return;
        }
        transform.GetChild(1).GetChild(0).
            GetComponent<TextMeshProUGUI>().text = card.data.keyword;

        transform.GetChild(3).GetComponent<Image>().sprite = card.data.image;

        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.cardName;

        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.description;

        transform.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.attack.ToString();
        transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.speed.ToString();
        transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.armor.ToString();
        transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.hitpoint.ToString();
        transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.foodPrice.ToString();
        transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.industryPrice.ToString();
        transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.MagicPrice.ToString();

        //transform.SetParent(CardsMannager.Instance.hand.transform);

        gameObject.SetActive(true);
    }

    internal static CardDisplayer GetDisplayer(int ID)
    {
        return CardDisplayerRegestry[ID];
    }

    public void OnLeftClick()
    {
        IClickable CurrentSelectedID = GameManager.CurrentSelected;
        if((object)CurrentSelectedID != this && CurrentSelectedID != null){
            //something else is selected
            //clear selection
            CurrentSelectedID.OnDeselect();
        }

        //Select this displayer
        OnSelect();
    }

    public void OnRightClick()
    {
        Debug.Log("How you managed that?");
    }

    public void OnSelect()
    {
        GameManager.CurrentSelected = this;
        transform.localScale = new Vector3(1.2f, 1.2f,1.2f);
    }

    public void OnDeselect()
    {
        GameManager.CurrentSelected = null;
        transform.localScale = new Vector3(.6f, .6f, .6f);
    }
}
