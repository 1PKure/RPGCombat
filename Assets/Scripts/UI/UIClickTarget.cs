using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickTarget : MonoBehaviour, IPointerClickHandler
{
    private CharacterBase targetCharacter;
    private System.Action<CharacterBase> onClickAction;

    public void Initialize(CharacterBase character, System.Action<CharacterBase> action)
    {
        targetCharacter = character;
        onClickAction = action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickAction?.Invoke(targetCharacter);
    }
}

