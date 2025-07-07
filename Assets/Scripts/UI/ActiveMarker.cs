using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMarker : MonoBehaviour
{
    public static ActiveMarker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //gameObject.SetActive(false);
    }

    public void SetUIIndicator(RectTransform uiSlot)
    {
        transform.SetParent(uiSlot);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}

