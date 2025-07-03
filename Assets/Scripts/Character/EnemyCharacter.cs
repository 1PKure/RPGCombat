using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    public override void PerformAction(System.Action onActionComplete)
    {
        StartCoroutine(EnemyTurnCoroutine(onActionComplete));
    }

    private IEnumerator EnemyTurnCoroutine(System.Action onActionComplete)
    {
        Debug.Log($"{characterName} empieza su turno.");
        for (int i = 0; i < stats.speed; i++)
        {
            Vector2Int[] directions = {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
            };

            Vector2Int newPos = gridPosition;
            Vector2Int chosenDir = directions[Random.Range(0, directions.Length)];
            Vector2Int potential = gridPosition + chosenDir;

            if (GameManager.Instance.mapView.IsAValidPosition(potential))
            {
                newPos = potential;
                gridPosition = newPos;
                transform.position = GameManager.Instance.mapView.GetWorldPosition(newPos);
                yield return new WaitForSeconds(0.1f); // Pausa para visualizar movimiento
            }
        }

        yield return new WaitForSeconds(0.3f);

        List<PlayerCharacter> players = new List<PlayerCharacter>(FindObjectsOfType<PlayerCharacter>());
        players.RemoveAll(p => !p.IsAlive());

        if (players.Count == 0)
        {
            Debug.Log($"{characterName} no encontró jugadores.");
            onActionComplete?.Invoke();
            yield break;
        }

        PlayerCharacter closest = GetClosestPlayer(players);

        int dist = Mathf.Abs(gridPosition.x - closest.gridPosition.x) + Mathf.Abs(gridPosition.y - closest.gridPosition.y);

        bool canAttack =
            (!stats.isRanged && dist == 1) ||
            (stats.isRanged && dist > 1 && dist <= stats.rangedRange);

        if (canAttack)
        {
            Debug.Log($"{characterName} ataca a {closest.characterName} (distancia: {dist})");

            int damage = stats.isRanged ? stats.rangedDamage : stats.meleeDamage;
            closest.TakeDamage(damage);

            if (!closest.IsAlive())
                Destroy(closest.gameObject);

            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            Debug.Log($"{characterName} no pudo atacar.");
        }

        onActionComplete?.Invoke();
    }

    private PlayerCharacter GetClosestPlayer(List<PlayerCharacter> players)
    {
        PlayerCharacter closest = null;
        int minDist = int.MaxValue;

        foreach (var player in players)
        {
            int dist = Mathf.Abs(gridPosition.x - player.gridPosition.x) + Mathf.Abs(gridPosition.y - player.gridPosition.y);

            if (dist < minDist)
            {
                minDist = dist;
                closest = player;
            }
            else if (dist == minDist && Random.value > 0.5f)
            {
                closest = player;
            }
        }

        return closest;
    }
}

