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

        transform.Find("Image").GetComponent<Image>().sprite = card.data.image;

        transform.Find("Header").GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.cardName;

        transform.Find("Tail").GetChild(0).
            GetComponent<TextMeshProUGUI>().text = card.data.cardType.ToString();

        transform.Find("Description").GetChild(0).GetComponent<TextMeshProUGUI>().text = card.description;
        transform.Find("Description").GetComponent<TooltipTrigger>().SetTextFromCard(card.data.keywords);

        if(card.data.cardType == CardData.CardType.Creature || 
        card.data.cardType == CardData.CardType.Town || 
        card.data.cardType == CardData.CardType.Fort || 
        card.data.cardType == CardData.CardType.worker )
        {

            if(card.speed != 0){
                ActivateAndShowText(transform.Find("Speed Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.speed.ToString());
            }
            else{
                transform.Find("Speed Indicator").gameObject.SetActive(false);
            }
            ActivateAndShowText(transform.Find("Attack Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.attack.ToString());
            ActivateAndShowText(transform.Find("Armor Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.armor.ToString());
            ActivateAndShowText(transform.Find("Hitpoint Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.hitpoint.ToString());
        }
        else{
            transform.Find("Attack Indicator").gameObject.SetActive(false);
            transform.Find("Armor Indicator").gameObject.SetActive(false);
            transform.Find("Speed Indicator").gameObject.SetActive(false);
            transform.Find("Hitpoint Indicator").gameObject.SetActive(false);
        }

            ActivateAndShowText(transform.Find("Food Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() ,card.foodPrice.ToString());
            ActivateAndShowText(transform.Find("Industry Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.industryPrice.ToString());
            ActivateAndShowText(transform.Find("Magic Indicator").GetChild(0).GetComponent<TextMeshProUGUI>() , card.MagicPrice.ToString());          


        transform.SetParent(UIController.Instance.handGUI.transform);

        gameObject.SetActive(true);
    }

    void ActivateAndShowText(TextMeshProUGUI text  , string value){
        text.text = value; 
        text.gameObject.SetActive(true);
    }

}
