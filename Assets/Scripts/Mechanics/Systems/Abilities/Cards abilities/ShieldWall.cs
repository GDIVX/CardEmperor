using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Mechanics.Components.Effects;

public class ShieldWall : CardAbility
{
    protected override void OnStart()
    {
    }


    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature creature = Creature.GetCreatureByPosition(targetPosition);

        if(creature == null) return false;
        
        WorldTile tile = WorldController.Instance.world[targetPosition.x , targetPosition.y];

        int vigorValue = 0;

        foreach (var n in tile.GetNeighbors())
        {
            if(n.CreatureID !=0){
                if(Creature.GetCreature(n.CreatureID).Player.IsMain()){
                    vigorValue++;
                }
            }
        }

        creature.AddEffect(new Vigor(vigorValue));
        HandleRemoval(ID);

        return true;
    }

        public override bool isPlayableOnTile(WorldTile tile)
    {
        return tile.CreatureID !=0;
    }

}
