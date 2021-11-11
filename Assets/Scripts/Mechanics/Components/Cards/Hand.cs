using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Stack<CardDisplayer> disabledCards{get{return _disabledCardsDisplayers;}}
    public List<CardDisplayer> cardDisplayers{get{return _cardDisplayers;}}
    public int Count{get => _cardDisplayers.Count;}
    [SerializeField]
    float spacing = 95f;
    float oldSpacing;
    List<CardDisplayer> _cardDisplayers;
    Stack<CardDisplayer> _disabledCardsDisplayers;

    private void Awake() {
        oldSpacing = spacing;
        _cardDisplayers = new List<CardDisplayer>();
        _disabledCardsDisplayers = new Stack<CardDisplayer>();
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
            display = GameObject.Instantiate(display);
            cardDisplayer = display.GetComponent<CardDisplayer>();
        }

        cardDisplayer.SetID(card.ID);
        display.transform.SetParent(gameObject.transform);
        _cardDisplayers.Add(cardDisplayer);
        cardDisplayer.SetDisplayActive(true);

        RearrangeCards(spacing);
    }

    internal int GetCardIDByIndex(int index)
    {
        return GetCardDisplayerByIndex(index).ID;
    }

    internal CardDisplayer GetCardDisplayerByIndex(int index)
    {
        return _cardDisplayers[index];
    }

    public bool IsEmpty(){
        return _cardDisplayers.Count > 0;
    }

    internal void RemoveCard(int ID)
    {
        if(!Has(ID)){
            Debug.LogError("Trying to remove a card that is not in the hand");
            return;
        }
        CardDisplayer displayer = CardDisplayer.GetDisplayer(ID);
        displayer.Clear();
        displayer.SetDisplayActive(false);
        _cardDisplayers.Remove(displayer);
        _disabledCardsDisplayers.Push(displayer);
        RearrangeCards();
    }


    private void RearrangeCards(float spacing)
    {
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
            _cardDisplayers[i].transform.position = new Vector3(x,y,z);
            x += spacing;
        }
    }

    public bool Has(int ID){
        foreach (CardDisplayer displayer in _cardDisplayers)
        {
            if(displayer.ID == ID){
                return true;
            }
        }

        return false;
    }

    void RearrangeCards(){
        RearrangeCards(spacing);
    }

    
}
