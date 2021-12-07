using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Show a GUI of the card with no user input
/// </summary>
[System.Serializable]

public class CardGUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int ID{get{return _ID;}}
    
    [ShowInInspector]
    protected int _ID;
    public virtual void OnPointerClick(PointerEventData eventData){}
    public virtual void OnPointerEnter(PointerEventData eventData){}
    public virtual void OnPointerExit(PointerEventData eventData){}

    public static GameObject Create(){
        
        GameObject _gameObject = CardsMannager.Instance.cardTamplate;
        _gameObject = GameObject.Instantiate(_gameObject);

        return _gameObject;
    }

    public virtual void SetID(int ID){
        this._ID = ID;
    }

    public virtual void Clear(){
        _ID = 0;
    }

    public void Reload(){
        if(isActiveAndEnabled){
            ShowDisplay();
        }
    }

    public void SetDisplayActive(bool isActive){
        if(isActive){
            ShowDisplay();
        }
        else{
            gameObject.SetActive(false);
        }
    }

    protected void ShowDisplay()
    {

        if(ID == 0){
            Debug.LogError("Did not set ID for CardGUI");
            return;
        }

        Card card = Card.GetCard(ID);
        
        if(card == null){
            Debug.LogError("card is not set to CardGUI");
            return;
        }

        transform.GetChild(0).GetComponent<Image>().sprite = card.data.image;

        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.cardName;

        transform.GetChild(2).GetChild(0).
            GetComponent<TextMeshProUGUI>().text = card.data.cardType.ToString();

        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.description;
        transform.GetChild(3).GetComponent<TooltipTrigger>().SetTextFromCard(card.data.keywords);

        if(card.data.cardType == CardData.CardType.Creature || 
        card.data.cardType == CardData.CardType.Town || 
        card.data.cardType == CardData.CardType.Fort || 
        card.data.cardType == CardData.CardType.worker ){

            if(card.speed != 0){
                ActivateAndShowText(transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>() , card.speed.ToString());
            }
            else{
                transform.GetChild(9).gameObject.SetActive(false);
            }
            ActivateAndShowText(transform.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>() , card.attack.ToString());
            ActivateAndShowText(transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>() , card.armor.ToString());
            ActivateAndShowText(transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>() , card.hitpoint.ToString());
        }
        else{
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
        }

            ActivateAndShowText(transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>() ,card.foodPrice.ToString());
            ActivateAndShowText(transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>() , card.industryPrice.ToString());
            ActivateAndShowText(transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>() , card.MagicPrice.ToString());          


        transform.SetParent(UIController.Instance.handGUI.transform);

        gameObject.SetActive(true);
    }

    void ActivateAndShowText(TextMeshProUGUI text  , string value){
        text.text = value; 
        text.gameObject.SetActive(true);
    }

}
