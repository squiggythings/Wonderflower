using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (SpriteAnimator))]
public class SimpleAnimation : MonoBehaviour
{
    public List<AnimationFrame> basicAnimation;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (GetComponent<SpriteAnimator>() != null)
        {
            GetComponent<SpriteAnimator>().playAnimation(basicAnimation);
        }
    }
}
