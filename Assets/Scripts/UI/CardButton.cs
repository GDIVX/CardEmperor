using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Display a GUI of a card and act as a button
/// </summary>
public class CardButton : CardGUI
{
    Tweener tweener;
    Action<Card> action;
    public static new GameObject Create(){
        GameObject _gameObject = CardGUI.Create();
        CardButton btn = _gameObject.AddComponent<CardButton>();

        btn.tweener = _gameObject.AddComponent<Tweener>();
        btn.tweener.targetScale = new Vector3(1.2f , 1.2f, 1);
        btn.tweener.time = .5f;
        btn.tweener.easeType = LeanTweenType.easeInOutBack;

        return _gameObject;
    }
    public static GameObject Create(Action<Card> action , int CardID){
        GameObject _gameObject = CardButton.Create();
        CardButton btn = _gameObject.GetComponent<CardButton>();
        btn.action += action;
        btn._ID = CardID;
        
        return _gameObject;
    }


    public void SetAction(Action<Card> action){
        this.action = action;
    
    
    }

    public void AddAction(Action<Card> action){
        this.action += action;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        action?.Invoke(Card.GetCard(ID));
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        tweener.ScaleFocus();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        tweener.UnscaledFocus();
    }

}
