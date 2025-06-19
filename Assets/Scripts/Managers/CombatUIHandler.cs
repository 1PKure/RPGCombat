using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIHandler : MonoBehaviour
{
    public void OnAttack1()
    {
        CombatManager.Instance.PlayerAttack(3);
    }

    public void OnAttack2()
    {
        CombatManager.Instance.PlayerAttack(5);
    }
}

