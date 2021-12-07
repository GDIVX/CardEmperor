using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store useful delegates and callbacks
/// </summary>
public static class GlobalDelegates
{
    public static Action<Creature, Creature> OnSuccessfulAttack;
    public static Action<Creature, Creature> OnAttackBlocked;
    public static Action<Creature, Creature> OnAttackAttempt;

}
