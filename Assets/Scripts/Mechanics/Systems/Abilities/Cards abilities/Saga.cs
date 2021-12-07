using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class Saga : CardAbility
{

    int value;
    protected override void OnStart()
    {
        value = Card.GetCard(ID).data.parm1;
        CardsMannager.OnDraw += OnDraw;
    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        GameManager.Instance.capital.AddEffect(new Omen(value));
        value = 0;
        HandleRemoval(ID);
        return true;
    }

    void OnDraw(Card card){
        if(card.ID == ID){
            value += card.data.parm2;
            card.description = card.data.description.Replace("_X_" , value.ToString());
            CardDisplayer.GetDisplayer(ID).Reload();
        }
    }
}
