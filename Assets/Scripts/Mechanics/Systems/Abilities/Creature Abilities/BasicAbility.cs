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
        int distance = WorldController.DistanceOf(creature.position, target.position);

        if (distance <= creature.attackRange)
        {
            int damage = GetAttackDamage(creature, target);

            target.ToastAttackFormated(damage, target.hitpoints);

            if (damage > 0)
            {
                //attack passed
                GlobalDelegates.OnSuccessfulAttack?.Invoke(creature, target);
                creature.OnAttackPassed(damage);
                target.TakeDamage(damage);
            }
            else
            {
                //attack blocked
                GlobalDelegates.OnAttackBlocked?.Invoke(creature, target);
                target.OnAttackBlocked(damage);
            }
            GlobalDelegates.OnAttackAttempt?.Invoke(creature, target);
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
