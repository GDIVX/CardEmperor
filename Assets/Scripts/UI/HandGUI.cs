using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandGUI : MonoBehaviour 
{
    public Stack<CardDisplayer> disabledCards{get{return _disabledCardsDisplayers;}}
    public List<CardDisplayer> cardDisplayers{get{return _cardDisplayers;}}

    
    

    [SerializeField]
    [OnValueChanged("RearrangeCards")]
    public float spacing = 95f;
    float oldSpacing;
    [ShowInInspector]
    List<CardDisplayer> _cardDisplayers;
    [ShowInInspector]
    Stack<CardDisplayer> _disabledCardsDisplayers;
    Tweener tweener;
    

    DisplayScale displayScale;

        private void Awake() {
        oldSpacing = spacing;
        _cardDisplayers = new List<CardDisplayer>();
        _disabledCardsDisplayers = new Stack<CardDisplayer>();

        tweener = GetComponent<Tweener>();
    }


    public void AddCard(Card card){
        if(card == null){
            Debug.LogWarning("Trying to add a null card to hand");
            return;
        }
        GameObject display; 
        CardDisplayer cardDisplayer;
        
        if(disabledCards.Count != 0){
            cardDisplayer = disabledCards.Pop();
            display = cardDisplayer.gameObject;
        }
        else{
            display = CardDisplayer.Create();
            cardDisplayer = display.GetComponent<CardDisplayer>();
        }

        cardDisplayer.SetID(card.ID);
        display.transform.SetParent(gameObject.transform);
        _cardDisplayers.Add(cardDisplayer);
        cardDisplayer.SetDisplayActive(true);

        RearrangeCards(spacing);
    }

        internal void RemoveCard(int ID)
    {

        CardDisplayer displayer = CardDisplayer.GetDisplayer(ID);
        if(displayer == null){
            Debug.LogWarning($"Can't find displayer of card{Card.GetCard(ID)}");
            RearrangeCards(spacing);
            return;
        }
        displayer.Clear();
        displayer.SetDisplayActive(false);
        _cardDisplayers.Remove(displayer);
        _disabledCardsDisplayers.Push(displayer);
        RearrangeCards(spacing);
    }

    public void RearrangeCards(float spacing)
    {
        if(!Application.isPlaying){return;}
        float totalHandLength = _cardDisplayers.Count * spacing;

        if(totalHandLength >= transform.parent.GetComponent<RectTransform>().rect.width){
            RearrangeCards(spacing - 5);
            return;
        }

        float x = transform.position.x -(totalHandLength / 2);
        float y = transform.position.y;
        float z = transform.position.z;

        for (int i = 0; i < _cardDisplayers.Count; i++)
        {
            _cardDisplayers[i].transform.LeanMove(new Vector3(x,y,z) , .2f).setEase(LeanTweenType.easeInOutSine);
            _cardDisplayers[i].transform.SetSiblingIndex(i);
            x += spacing;
        }
    }

    public void RearrangeCards(){
        RearrangeCards(spacing);
    }

    public void ArrangeCard(int ID){
        if(CardsMannager.Instance.hand.Has(ID)){
            CardDisplayer displayer = CardDisplayer.GetDisplayer(ID);
            if(displayer == null){
                Debug.LogError("Can't find card displayer for ID "+ID);
                return;
            }
            int index = _cardDisplayers.IndexOf(displayer);
            _cardDisplayers[index].transform.SetSiblingIndex(index);

        }
        else{
            Debug.LogError("Trying to arrange a card that isn't in hand");
        }
    }


    public void SetToMixedView(){
        if(displayScale == DisplayScale.HandView){
            tweener.UnfocusedOn();
        }
        tweener.MoveToOrigin();
        displayScale = DisplayScale.MixedView;
    }

    public void SetToMapView(){
        if(displayScale == DisplayScale.HandView){
            tweener.UnfocusedOn();
        }
        tweener.MoveAwayFromOrigin();
        displayScale = DisplayScale.MapView;
    }

    public void SetToHandView(){
        tweener.FocusOn();
        displayScale = DisplayScale.HandView;
    }

    public void ScaleDown(){
        switch (displayScale){
            case DisplayScale.HandView:
                SetToMixedView();
                break;

            case DisplayScale.MixedView:
                SetToMapView();
                break;
            default:
                return;
        }
    }
    public void ScaleUp(){
        switch (displayScale){
            case DisplayScale.MapView:
                SetToMixedView();
                break;

            case DisplayScale.MixedView:
                SetToHandView();
                break;
            default:
                return;
        }
    }

    enum DisplayScale{MapView , MixedView , HandView}
}
