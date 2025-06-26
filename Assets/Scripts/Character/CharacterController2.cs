using System.Collections.Generic;
using UnityEngine;

public class CharacterController2 : MonoBehaviour, IUnit
{
    [SerializeField] private MapView mapView;
    [SerializeField] private int speed = 1;
    public string UnitName => gameObject.name;
    public bool IsPlayer => true;
    public int Speed => speed;
    public int Health => currentHealth;
    public bool IsDead => currentHealth <= 0;
    private Vector2Int characterPosition;
    private bool myTurn = false;
    private int currentHealth = 15;

    private void Start()
    {
        if (mapView == null)
        {
            Initialize(characterPosition);
        }
        else
        {
            Debug.LogWarning("MapView no asignado en CHaracterController2");
        }
        
    }

    public void StartTurn()
    {
        myTurn = true;
        Debug.Log(UnitName + " comienza su turno.");
    }

    public void EndTurn()
    {
        myTurn = false;
    }

    private void Update()
    {
        if (!myTurn) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(UnitName + " realiza su acción.");
            MoveCharacter();
        }

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
