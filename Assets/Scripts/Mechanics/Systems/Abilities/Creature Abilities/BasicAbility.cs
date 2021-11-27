using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbility : CreatureAbility
{
    public BasicAbility(int ID) : base(ID)
    {
    }

    /// <summary>
    /// Attack the target using base attacks rules
    /// </summary>
    /// <param name="target">The target to attack</param>
    public override void ActionOnEnemyCreature(Creature target)
    {
        //Find the attacking creature
        Creature creature = Creature.GetCreature(ID);
        //Find the distance between the creature and the target
        int distance = WorldController.DistanceOf(creature.position , target.position);
        
        if(distance <= creature.attackRange){
            //Roll attack dice
            int attackRoll = Dice.Roll(creature.Attack);
            int attackAvarage = Mathf.RoundToInt(attackRoll / creature.Attack);
            //Roll armor dice
            //Missing often is not fun, push the avarage roll to be lower then the attack roll
            int armorRoll = Dice.Roll(target.Armor ,Mathf.RoundToInt( attackAvarage /2) );

            int damage = attackRoll - armorRoll;

            if(creature.Player.IsMain()){
                //nudge the numbers slightly in favour of the player
                damage  = Mathf.RoundToInt(damage*Random.Range(1,1.25f));
            }
            else{
                damage  = Mathf.RoundToInt(damage*Random.Range(0.25f,1.25f));
            }

            target.ToastAttackFormated(damage , target.Hitpoint);

            if(damage > 0){
                //attack passed
                creature.OnAttackPassed(damage);
                target.TakeDamage(damage);
            }else{
                //attack blocked
                target.OnAttackBlocked(damage);
            }
        }
    }

    public override void ActionOnFriendlyCreature(Creature target)
    {
        //Do nothing
    }

    public override void OnDeath()
    {
        //Do nothing
    }

    public override void OnSpawn()
    {
        //Do nothing
    }
    protected override void OnAbilityTriggered()
    {
        // no triggers
    }
}
