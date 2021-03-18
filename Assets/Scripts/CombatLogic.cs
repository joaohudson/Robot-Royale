using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogic
{

    public static void TakeDamage(CharacterState state, int damage, float criticalChance)
    {
        if(Random.Range(0f, 1f) <= criticalChance)
            damage *= 2;
        
        state.Health -= damage;
    }
}
