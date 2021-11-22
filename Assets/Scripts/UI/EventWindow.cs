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
    List<CardButton> cardButtons = new List<CardButton>();


    [Button]
    public void Show(){

        if(cardButtons.Count > 0){
            foreach (CardButton btn in cardButtons)
            {
                btn.SetDisplayActive(true);
                btn.gameObject.SetActive(true);
                btn.gameObject.transform.SetParent(cardHolder);
            }
        }

        tweener.Showcase();
    }

    [Button]
    public void Hide(){
        if(cardButtons.Count > 0){
            foreach (Transform c in cardHolder)
            {
                c.gameObject.SetActive(false);
            }
        }
        tweener.Hide();
    }

    public void SetEvent(GameEvent gameEvent){
        title.text = gameEvent.title;

        if(gameEvent.GetType() == typeof(CardEvent)){
            CardEvent cardEvent = gameEvent as CardEvent;

            foreach (var card in cardEvent.cards)
            {
                CardButton btn = null;
                if(UIController.GetDisabledChildrenCount(cardHolder) > 0){
                    btn = UIController.GetDisabledChild(cardHolder).GetComponent<CardButton>();
                    btn.SetID(card.ID);
                }
                else{
                    btn = CardButton.Create(cardEvent.cardAction , card.ID).GetComponent<CardButton>();
                    cardButtons.Add(btn);
                }
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
