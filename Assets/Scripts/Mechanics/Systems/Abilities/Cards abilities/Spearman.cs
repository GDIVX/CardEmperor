using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using UnityEngine;

public class Spearman : CreatureMaker
{
    protected override bool _Activate(Vector3Int targetPosition)
    {
        if (base._Activate(targetPosition))
        {

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

            creature.AddEffect(new Spikes(Card.GetCard(ID).data.parm1 + formationCount));
            return true;
        }
        return false;
    }
}
