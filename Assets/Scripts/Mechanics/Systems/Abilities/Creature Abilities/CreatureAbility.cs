using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CreatureAbility 
{
    public int ID;

    protected CreatureAbility(int ID)
    {
        this.ID = ID;
    }

    public abstract void ActionOnEnemyCreature(Creature target);
    public abstract void ActionOnFriendlyCreature(Creature target);
    public abstract void OnSpawn();
    public abstract void OnDeath();
    protected abstract void OnAbilityTriggered();
}
