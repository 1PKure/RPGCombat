using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    private CharacterBase stats;
    public void Initialize(Vector2Int pos)
    {
        this.characterName = "Enemy";
        this.maxHealth = 10;
        this.currentHealth = 10;
        this.gridPosition = pos;
        this.speed = 1;

        transform.position = GameManager.Instance.mapView.GetWorldPosition(pos);
    }
    public override void PerformAction(System.Action onActionComplete)
    {
        StartCoroutine(EnemyTurnCoroutine(onActionComplete));
        onActionComplete?.Invoke();
    }

    private IEnumerator EnemyTurnCoroutine(System.Action onActionComplete)
    {
        if (GameManager.Instance.turnManager.GameEnded)
        {
            onActionComplete?.Invoke();
            yield break;
        }
        UIManager.Instance.ShowTurnMessage($"{characterName} Starts turn.");

        for (int i = 0; i < speed; i++)
        {
            Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

            Vector2Int chosenDir = directions[Random.Range(0, directions.Length)];
            Vector2Int potential = gridPosition + chosenDir;

            if (GameManager.Instance.mapView.IsAValidPosition(potential))
            {
                gridPosition = potential;
                transform.position = GameManager.Instance.mapView.GetWorldPosition(potential);
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.3f);

        List<PlayerCharacter> players = new List<PlayerCharacter>(FindObjectsOfType<PlayerCharacter>());
        players.RemoveAll(p => !p.IsAlive());

        if (players.Count == 0)
        {
            UIManager.Instance.ShowMessage($"{characterName} not found any players");
            onActionComplete?.Invoke();
            yield break;
        }

        PlayerCharacter closest = GetClosestPlayer(players);

        int distance = Mathf.Abs(gridPosition.x - closest.gridPosition.x) + Mathf.Abs(gridPosition.y - closest.gridPosition.y);

        if (distance <= 1 || (Mathf.Abs(gridPosition.x - closest.gridPosition.x) <= 1 && Mathf.Abs(gridPosition.y - closest.gridPosition.y) <= 1))
        {
            int damage = 3;
            closest.TakeDamage(damage);
            UIManager.Instance.ShowMessage($"{characterName} attack {closest.characterName} for {damage} of damage.");
        }
        else if (distance <= 3)
        {
            int damage = 1;
            closest.TakeDamage(damage);
            UIManager.Instance.ShowMessage($"{characterName} shoot {closest.characterName} for {damage} of damage.");
        }

        GameManager.Instance.turnManager.CheckEndConditions();
        yield return new WaitForSeconds(0.3f);
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

