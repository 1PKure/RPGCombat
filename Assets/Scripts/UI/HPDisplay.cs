using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    private CharacterBase owner;

    private void Start()
    {
        owner = GetComponentInParent<CharacterBase>();
        UpdateHP();
    }

    public void UpdateHP()
    {
        hpText.text = $"HP: {owner.currentHealth}";
    }
}
