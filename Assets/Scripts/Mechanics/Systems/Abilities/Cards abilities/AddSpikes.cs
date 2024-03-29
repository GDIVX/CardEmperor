using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class AddSpikes : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID !=0;
    }
    
    public AddSpikes(){
        
    }

    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);
        if (creature == null) return false;
        Card card = Card.GetCard(ID);
        if (card == null) return false;

        creature.AddEffect(new Spikes(card.data.parm1));

        HandleRemoval(ID);

        return true;
    }
}
