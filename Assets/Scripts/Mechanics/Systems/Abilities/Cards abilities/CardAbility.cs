using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class CardAbility {

[HideInInspector]
    public int ID{get =>_ID;}
    
    public abstract void Activate(Vector3Int targetPosition);
    public abstract void Activate(CardDisplayer targetCard);
    protected abstract void OnStart();
    protected abstract void OnTriggerEnabled();

    static Dictionary<int,CardAbility> regestry = new Dictionary<int, CardAbility>();
    private int _ID;

    public CardAbility(){
        OnStart();
    }

    public void Register(int ID){
        _ID = ID;
        regestry.Add(ID , this);
    }

    public static CardAbility GetAbility(int ID){
        return regestry[ID];
    }

    public static bool CanAfford(int ID){
                Card card = Card.GetCard(ID);
        Player player = GameManager.Instance.CurrentTurnOfPlayer;

        int leftoverFood = player.foodPoints.Value - card.foodPrice;
        int leftoverIndustry = player.industryPoints.Value - card.industryPrice;
        int leftoverMagic = player.magicPoints.Value - card.MagicPrice;

        if(leftoverFood < 0 || leftoverIndustry < 0 || leftoverMagic < 0){
            //we can't afford to play this card
            Debug.Log("Can't afford this card");
            return false;
        }
        player.foodPoints.SetValue(leftoverFood);
        player.industryPoints.SetValue(leftoverIndustry);
        player.magicPoints.SetValue(leftoverMagic);
        return true;
    }
    
}
