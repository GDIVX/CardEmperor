using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Show a GUI of the card with no user input
/// </summary>

public class CardGUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
        public int ID{get{return _ID;}}

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
        transform.GetChild(1).GetChild(0).
            GetComponent<TextMeshProUGUI>().text = card.data.cardType.ToString();

        transform.GetChild(3).GetComponent<Image>().sprite = card.data.image;

        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.cardName;

        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.data.description;
        transform.GetChild(2).GetComponent<TooltipTrigger>().SetTextFromCard(card.data.keywords);

        transform.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.attack.ToString();
        transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.speed.ToString();
        transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.armor.ToString();
        transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.hitpoint.ToString();
        transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.foodPrice.ToString();
        transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.industryPrice.ToString();
        transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.MagicPrice.ToString();


        transform.SetParent(UIController.Instance.handGUI.transform);

        gameObject.SetActive(true);
    }


}
