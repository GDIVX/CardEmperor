using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using Assets.Scripts.UI;
using UnityEngine;


[System.Serializable]
public abstract class CardAbility
{

    [HideInInspector]
    public int ID { get => _ID; }

    protected abstract bool _Activate(Vector3Int targetPosition);
    protected abstract void OnStart();

    public void Activate(Vector3Int targetPosition)
    {
        if (CanAfford(ID))
        {
            if(_Activate(targetPosition))
                PayForCard(ID);
            else
                Prompt.ToastCenter("<color=blue>Can't Play This Card</color>", 1); 
        }
        else
        {
            Prompt.ToastCenter("<color=blue>Can't Afford To Play The Card</color>", 1);
        }
    }

    static Dictionary<int, CardAbility> regestry = new Dictionary<int, CardAbility>();
    private int _ID;

    public CardAbility()
    {
        OnStart();
    }

    public void Register(int ID)
    {
        _ID = ID;
        regestry.Add(ID, this);
    }

    public static CardAbility GetAbility(int ID)
    {
        return regestry[ID];
    }

    public static bool CanAfford(int ID)
    {
        Card card = Card.GetCard(ID);
        Player player = GameManager.Instance.CurrentTurnOfPlayer;
        if (player == null)
        {
            Debug.LogError("Trying to play a card when its null player turn");
            return false;
        }

        int leftoverFood = player.foodPoints.Value - card.foodPrice;
        int leftoverIndustry = player.industryPoints.Value - card.industryPrice;
        int leftoverMagic = player.magicPoints.Value - card.MagicPrice;

        if (leftoverFood < 0 || leftoverIndustry < 0 || leftoverMagic < 0)
        {
            //we can't afford to play this card
            return false;
        }

        return true;
    }

    public static void PayForCard(int ID){

        Card card = Card.GetCard(ID);
        Player player = Player.Main;

        int leftoverFood = player.foodPoints.Value - card.foodPrice;
        int leftoverIndustry = player.industryPoints.Value - card.industryPrice;
        int leftoverMagic = player.magicPoints.Value - card.MagicPrice;

        player.foodPoints.SetValue(leftoverFood);
        player.industryPoints.SetValue(leftoverIndustry);
        player.magicPoints.SetValue(leftoverMagic);
    }

    public static void HandleRemoval(int ID)
    {
        if(!CardsMannager.Instance.hand.Has(ID)){
            //card is not in hand
            //Most likely the ability was called off hand by another card
            //Do not remove or discard, let the calling card handle removal
            return;
        }
        bool exile = Card.GetCard(ID).data.Exile;
        if (exile)
        {
            RemoveAndExile(ID);
        }
        else
        {
            RemoveAndDiscard(ID);
        }
    }

    public static void RemoveAndDiscard(int ID)
    {
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.discardPile.Drop(Card.GetCard(ID));
    }

    public static void RemoveAndExile(int ID)
    {
        CardsMannager.Instance.hand.RemoveCard(ID);
        CardsMannager.Instance.exilePile.Drop(Card.GetCard(ID));
    }

    public static void ForceExile(Card card){
        if(CardsMannager.Instance.hand.Has(card.ID))
            RemoveAndExile(card.ID);
        else if(!CardsMannager.Instance.exilePile.Has(card)){
            card.GetPile().Remove(card);
            CardsMannager.Instance.exilePile.Drop(card);
        }
    }

    public int GetFormationCount(Vector3Int targetPosition){
        
            WorldTile tile = WorldController.Instance.world[targetPosition.x, targetPosition.y];
            WorldTile[] tiles = tile.GetNeighbors();
            int formationCount = 0;

            foreach (var n in tiles)
            {
                if (n.CreatureID != 0)
                {
                    if (Creature.GetCreatureByPosition((Vector3Int)n.position).Player.IsMain())
                    {
                        formationCount++;
                    }
                }
            }

            return formationCount;
    }
}
