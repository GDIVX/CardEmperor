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

    protected int GetAttackDamage(Creature attacker , Creature defender){
        //Roll attack dice
        int attackRoll = Dice.Roll(attacker.attack) + attacker.damageBonus;
        int attackAvarage = Mathf.RoundToInt(attackRoll / attacker.attack);


        int damage = attackRoll - defender.armor;

        if(attacker.Player.IsMain()){
            //nudge the numbers slightly in favour of the player
            damage  = Mathf.RoundToInt(damage*Random.Range(1,1.25f));
        }
        else{
            damage  = Mathf.RoundToInt(damage*Random.Range(0.25f,1.25f));
        }

        return damage;
    }

    public abstract void ActionOnEnemyCreature(Creature target);
    public abstract void ActionOnFriendlyCreature(Creature target);
    public abstract void OnSpawn();
    public abstract void OnDeath();
    protected abstract void OnAbilityTriggered();
}
