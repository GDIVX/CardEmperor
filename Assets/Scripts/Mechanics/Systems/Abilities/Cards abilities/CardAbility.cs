using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Assets.Scripts.UI;
using UnityEngine;


[System.Serializable]
public abstract class CardAbility {

[HideInInspector]
    public int ID{get =>_ID;}
    
    protected abstract void _Activate(Vector3Int targetPosition);
    protected abstract void _Activate(CardDisplayer targetCard);
    protected abstract void OnStart();
    protected abstract void OnTriggerEnabled();

    public void Activate(Vector3Int targetPosition){
        if(CanAfford(ID)){
            _Activate(targetPosition);
        }
        else{
            Prompt.ToastCenter("<color=blue>Can't Afford To Play The Card</color>" , .8f);
        }
    }

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
        if(player == null){
            Debug.LogError("Trying to play a card when its null player turn");
        }

        int leftoverFood = player.foodPoints.Value - card.foodPrice;
        int leftoverIndustry = player.industryPoints.Value - card.industryPrice;
        int leftoverMagic = player.magicPoints.Value - card.MagicPrice;

        if(leftoverFood < 0 || leftoverIndustry < 0 || leftoverMagic < 0){
            //we can't afford to play this card
            return false;
        }
        player.foodPoints.SetValue(leftoverFood);
        player.industryPoints.SetValue(leftoverIndustry);
        player.magicPoints.SetValue(leftoverMagic);
        return true;
    }
    
}
