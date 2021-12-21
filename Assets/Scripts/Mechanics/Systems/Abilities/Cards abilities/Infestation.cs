using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class Infestation : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return true;
    }

    protected override void OnStart()
    {
        CardsMannager.OnDraw += OnDrawn;
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        HandleRemoval(ID);
        return true;
    }

    public void OnDrawn(Card drawnCard){
        if(drawnCard.ID == ID){
            Player.Main.foodPoints.SetValue(Player.Main.foodPoints.value - drawnCard.data.parm1);
        }
    }
}
