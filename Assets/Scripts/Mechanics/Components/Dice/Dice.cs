using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dice 
{
    /// <summary>
    /// Roll a group of 10 sided dices. 
    /// </summary>
    /// <param name="numberOfDices">The amount of dices to roll</param>
    /// <returns>A random number int from a normal distribution</returns>
    public static int Roll(int numberOfDices){
        if(numberOfDices <= 0){return 0;}
        int results = 0;

        for (var i = 1; i <= numberOfDices; i++)
        {
            int randomValue = Mathf.RoundToInt(Random.Range(1,10));
            results += randomValue;
        }

        return results;
    }
}
