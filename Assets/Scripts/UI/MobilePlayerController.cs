using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Terresquall;

public class MobilePlayerController : MonoBehaviour
{
    public float inputDelay = 0.3f;
    private float timer;
    private PlayerCharacter player;
    private VirtualJoystick joystick;



    void Update()
    {
        var joystick = VirtualJoystick.GetInstance();
        var current = GameManager.Instance.turnManager.CurrentCharacter as PlayerCharacter;
        if (joystick == null || current == null || !current.IsMyTurn())
        {
            timer = 0f;
            return;
        }
        if (player == null) player = FindObjectOfType<PlayerCharacter>();
        if (current == null || !current.IsMyTurn()) return;

        timer += Time.deltaTime;

        Vector2 input = joystick.GetAxis();

        Vector2Int moveDir = Vector2Int.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0.5f) moveDir = Vector2Int.right;
            else if (input.x < -0.5f) moveDir = Vector2Int.left;
        }
        else
        {
            if (input.y > 0.5f) moveDir = Vector2Int.up;
            else if (input.y < -0.5f) moveDir = Vector2Int.down;
        }

        if (moveDir != Vector2Int.zero && timer >= inputDelay)
        {
            timer = 0f;
            current.TryMoveInDirection(moveDir);
        }
    }
}
