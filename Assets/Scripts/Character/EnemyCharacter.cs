using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    public override void PerformAction(System.Action onActionComplete)
    {
        // Para pruebas automáticas: atacar al primer jugador disponible
        PlayerCharacter target = FindObjectOfType<PlayerCharacter>();
        if (target != null && target.IsAlive())
        {
            Debug.Log($"{characterName} attacks {target.characterName}");
            target.TakeDamage(5);
        }
        else
        {
            Debug.Log($"{characterName} has no players to attack.");
        }

        onActionComplete?.Invoke();
    }
}
