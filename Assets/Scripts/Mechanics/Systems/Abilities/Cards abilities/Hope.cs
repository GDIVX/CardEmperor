using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Mechanics.Components.Effects;
using Assets.Scripts.Mechanics.Systems.Players;
using UnityEngine;

public class Hope : CardAbility
{
    protected override void OnStart()
    {

    }

    protected override bool _Activate(Vector3Int targetPosition)
    {
        Creature[] allCreatures = Creature.GetAll(Player.Main.ID);

        foreach (Creature creature in allCreatures)
        {
            creature.AddEffect(new Toughness(5));
        }

        RemoveAndExile(ID);

        return true;
    }

}
