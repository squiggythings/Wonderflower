using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicBars : MonoBehaviour
{
    public SpriteRenderer top, bottom;
    private float length = 0;
    public int animationLength;
    public void Show()
    {
        StartCoroutine(animate(1));
    }

    public void Hide()
    {
        StartCoroutine(animate(-1));
    }


    public IEnumerator animate(int direction)
    {
        if (Mathf.Sign(length - 4) != direction)
            for (int i = 0; i < animationLength; ++i)
            {
                length += (float)direction * 8 / animationLength;
                top.size = new Vector2(224, (int)length);
                bottom.size = new Vector2(224, (int)length);
                yield return null;
            }
    }
}
