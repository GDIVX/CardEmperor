using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplayer : CardGUI ,IClickable
{
    public Action OnUpdateDisplayerListener;
    private static Dictionary<int, CardDisplayer> CardDisplayerRegestry = new Dictionary<int, CardDisplayer>();



    public override void SetID(int ID){
        base.SetID(ID);
        CardDisplayer.CardDisplayerRegestry[ID] = this;
    }

    public override void Clear(){
        CardDisplayer.CardDisplayerRegestry[_ID] = null;
        base.Clear();
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

    public void OnRightClick(){}

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
        
        GameManager.CurrentSelected = null;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnLeftClick();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        UIController.instance.handGUI.ArrangeCard(ID);
    }
}
