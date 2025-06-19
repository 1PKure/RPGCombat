using System.Collections.Generic;
using UnityEngine;

public class CharacterController2 : MonoBehaviour
{
    [SerializeField] private MapView mapView;
    [SerializeField] private int speed = 1;

    private Vector2Int characterPosition;

    private void Start()
    {
        Initialize(characterPosition);
    }

    private void Update()
    {
        MoveCharacter();
    }

    private void TryMove(int modX, int modY)
    {
        var posibleNewPosition = new Vector2Int(characterPosition.x + modX, characterPosition.y + modY);
        if (mapView.IsAValidPosition(posibleNewPosition))
        {
            characterPosition = posibleNewPosition;

            GameObject gridCell = mapView.Grid[characterPosition.y][characterPosition.x];
            transform.SetParent(gridCell.transform);
            transform.localPosition = Vector3.zero;

            if (mapView.IsWinningCell(characterPosition))
                Debug.Log("YOU WIN!!!");
        }
    }
   
    private void MoveCharacter()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(-speed, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(speed, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(0, speed);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(0,-speed);
        }
    }

    public void Initialize(Vector2Int characterPosition)
    {
        GameObject startGridCell = mapView.Grid[characterPosition.y][characterPosition.x];
        transform.SetParent(startGridCell.transform);
        transform.localPosition = Vector3.zero;
    }
}
