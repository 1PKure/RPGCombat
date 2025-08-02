using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    void AdjustBackgroundToCamera()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        Vector2 size = sr.sprite.bounds.size;

        transform.localScale = new Vector3(
            worldScreenWidth / size.x,
            worldScreenHeight / size.y,
            1
        );
    }

}
