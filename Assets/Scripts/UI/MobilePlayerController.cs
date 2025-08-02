using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class MobilePlayerController : MonoBehaviour
{
    public float inputDelay = 0.3f;
    private float timer;
    private PlayerCharacter player;

    private VirtualJoystick joystick;

    void Start()
    {
        joystick = VirtualJoystick.GetInstance();
        player = FindObjectOfType<PlayerCharacter>();
    }

    void Update()
    {
        //Debug.Log(joystick.GetAxis());
        if (player == null || !player.IsMyTurn()) return;

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
            player.TryMoveInDirection(moveDir);
        }
    }
}
