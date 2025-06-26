using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    private int stepsRemaining;
    private bool isMyTurn = false;

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
                    ShowActionPanel(); // Una vez que se mueve, habilitamos la acción
                }
            }
        }
    }

    public override void PerformAction(System.Action onActionComplete)
    {
        isMyTurn = true;
        stepsRemaining = speed;

        // Guardamos callback para cuando finalice su acción (por botón luego)
        GameManager.Instance.turnManager.RegisterPlayerCallback(this, onActionComplete);
    }

    public void EndPlayerTurn()
    {
        isMyTurn = false;
    }

    private void ShowActionPanel()
    {
        GameManager.Instance.UIManager.ShowActionsFor(this);
    }
}

