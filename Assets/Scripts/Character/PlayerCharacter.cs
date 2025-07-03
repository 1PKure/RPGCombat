using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    private int stepsRemaining;


    private void Update()
    {
        if (!isMyTurn || stepsRemaining <= 0) return;

        Vector2Int inputDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) inputDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) inputDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) inputDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) inputDirection = Vector2Int.right;

        if (inputDirection != Vector2Int.zero)
        {
            Vector2Int newPos = gridPosition + inputDirection;

            if (GameManager.Instance.mapView.IsAValidPosition(newPos))
            {
                gridPosition = newPos;
                transform.position = GameManager.Instance.mapView.GetWorldPosition(newPos);
                stepsRemaining--;

                if (stepsRemaining <= 0)
                {
                    ShowActionPanel();
                }
            }
        }
    }

    public override void PerformAction(System.Action onActionComplete)
    {
        isMyTurn = true;
        stepsRemaining = speed;
        ActiveMarker.Instance.SetTarget(transform);
        GameManager.Instance.turnManager.RegisterPlayerCallback(this, onActionComplete);
    }



    private void ShowActionPanel()
    {
        GameManager.Instance.UIManager.ShowActionsFor(this);
    }

    public List<EnemyCharacter> GetEnemiesInRange()
    {
        List<EnemyCharacter> enemies = new List<EnemyCharacter>();

        foreach (var enemy in FindObjectsOfType<EnemyCharacter>())
        {
            if (!enemy.IsAlive()) continue;

            int dist = Mathf.Abs(gridPosition.x - enemy.gridPosition.x) + Mathf.Abs(gridPosition.y - enemy.gridPosition.y);

            if (stats.isRanged)
            {
                if (dist > 1 && dist <= stats.rangedRange)
                    enemies.Add(enemy);
            }
            else
            {
                if (dist == 1)
                    enemies.Add(enemy);
            }
        }

        return enemies;
    }

    public List<CharacterBase> GetAlliesInHealRange()
    {
        List<CharacterBase> allies = new List<CharacterBase>();
        foreach (var ally in FindObjectsOfType<PlayerCharacter>())
        {
            if (!ally.IsAlive() || ally == this) continue;

            int dist = Mathf.Abs(gridPosition.x - ally.gridPosition.x) + Mathf.Abs(gridPosition.y - ally.gridPosition.y);
            if (dist <= stats.healRange)
                allies.Add(ally);
        }
        if (stats.canHeal)
            allies.Add(this);

        return allies;
    }

    private System.Action clickAction;

    private void OnMouseDown()
    {
        clickAction?.Invoke();
    }

    public override void OnClickedToReceiveAction(System.Action callback)
    {
        clickAction = callback;
    }
}

