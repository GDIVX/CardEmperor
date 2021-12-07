using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class Apocalypse : CardAbility
{

    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        GameManager.Instance.capital.AddEffect(new Omen(Card.GetCard(ID).data.parm1));
        GameManager.Instance.turnSequenceMannager.OnTurnComplete += DestroyCapital;
        HandleRemoval(ID);
        return true;
    }

    public void DestroyCapital(Turn turn){
        GameManager.Instance.capital.TakeDamage(Card.GetCard(ID).data.parm2);
    }


}
