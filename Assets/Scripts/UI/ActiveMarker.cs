using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMarker : MonoBehaviour
{
    public static ActiveMarker Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void SetTarget(Transform target)
    {
        gameObject.SetActive(true);
        transform.SetParent(target);
        transform.localPosition = Vector3.up * 1.5f;
    }

    public void Hide()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}

