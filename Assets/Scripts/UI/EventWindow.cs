using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using Assets.Scripts.Mechanics.Systems.Players;

public class EventWindow : MonoBehaviour
{
    [SerializeField]
    Tweener tweener;
    [SerializeField]
    Transform cardHolder;
    [SerializeField]
    TextMeshProUGUI title;
    [ShowInInspector]
    List<CardButton> cardButtons = new List<CardButton>();


    [Button]
    public void Show(){

        Debug.Log(cardButtons.Count);
        if(cardButtons.Count > 0){
            for (var i = 0; i < cardButtons.Count; i++)
            {
                cardButtons[i].SetDisplayActive(true);
                cardButtons[i].gameObject.transform.SetParent(cardHolder);
                cardButtons[i].gameObject.SetActive(true);
            }

        }

        tweener.Showcase();
    }

    internal void Clear()
    {
        if(cardButtons.Count > 0){
            foreach (Transform c in cardHolder)
            {
                c.gameObject.SetActive(false);
            }
        }
        cardButtons.Clear();
    }

    [Button]
    public void Hide(){
        Clear();
        tweener.Hide();
    }

    public void SetEvent(GameEvent gameEvent){
        title.text = gameEvent.title;

        if(gameEvent.GetType() == typeof(CardEvent)){
            CardEvent cardEvent = gameEvent as CardEvent;
            GameObject[] buttons = UIController.GetDisableChildren(cardHolder);

            for (var i = 0; i < cardEvent.cards.Count; i++)
            {
                CardButton btn = null;
                if(buttons.Length <= i){
                    //no aviable button
                    btn = CardButton.Create(cardEvent.cardAction , cardEvent.cards[i].ID).GetComponent<CardButton>();
                }
                else
                {
                    btn = buttons[i].GetComponent<CardButton>();
                    btn.SetID(cardEvent.cards[i].ID);
                    btn.SetAction(cardEvent.cardAction);
                    
                }
                cardButtons.Add(btn);
            }
        }
    }

    [Button]
    public void debug_test(){
        Card c1 , c2 , c3 , c4;
        c1 = Card.BuildCard("Capital" , Player.Main.ID);
        c2 = Card.BuildCard("Militia" , Player.Main.ID);
        c3 = Card.BuildCard("Village" , Player.Main.ID);
        c4 = Card.BuildCard("Militia" , Player.Main.ID);

        List<Card> list = new List<Card>(){c1,c2,c3,c4};

        CardEvent cardEvent = new CardEvent("Choose a card to add to your hand",list , (x)=>{
            CardsMannager.Instance.hand.AddCard(x);
            UIController.Instance.eventWindow.Hide();
        });

        SetEvent(cardEvent);
        Show();
    }
}
