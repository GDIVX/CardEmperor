using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This card ability can be used for curse cards that do nothing but to clutter up the deck
/// </summary>
public class EmptyCardAbility : CardAbility
{
    public override bool isPlayableOnTile(WorldTile tile)
    {
        return true;
    }

    protected override void OnStart()
    {}
        

    protected override bool _Activate(Vector3Int targetPosition)
    {
        HandleRemoval(ID);
        return true;
    }
}
