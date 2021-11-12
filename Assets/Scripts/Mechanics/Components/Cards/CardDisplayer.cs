﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplayer : MonoBehaviour , IClickable , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
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

    Vector3 originalScale;
    public void OnSelect()
    {
        if(GameManager.CurrentSelected == this) {return;}

        originalScale = transform.localScale;
        LeanTween.scale(gameObject , originalScale * 1.2f , .2f);
        GameManager.CurrentSelected = this;
    }

    public void OnDeselect()
    {
        if(GameManager.CurrentSelected != this) {return;}

        LeanTween.scale(gameObject , originalScale, .2f);
        
        PushDownHand();
        GameManager.CurrentSelected = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnLeftClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();

        IClickable selected = GameManager.CurrentSelected;
        CardDisplayer cast = selected as CardDisplayer;
        if(cast == null)
        {
            UIController.instance.handGUI.PushUp();
        }

    }

    void PushDownHand(){

        IClickable selected = GameManager.CurrentSelected;
        CardDisplayer cast = selected as CardDisplayer;
            Debug.Log(cast);
        if(cast == null)
        {
            UIController.instance.handGUI.PushDown();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIController.instance.handGUI.ArrangeCard(ID);
        PushDownHand();
    }
}
