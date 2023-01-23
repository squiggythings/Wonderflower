using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer sprite;

    private List<AnimationFrame> anim = new List<AnimationFrame>();
    private int i;
    private int currentFrame;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (anim.Count > 1)
        {
            if (i >= anim[currentFrame].FrameLength)
            {
                i = 0;
                ++currentFrame;
                if (currentFrame >= anim.Count) currentFrame = 0;
                sprite.sprite = anim[currentFrame].Sprite;
            }
            ++i;
        }
    }

    /// <summary>
    /// Plays a looping animation, starting from the beginning.
    /// </summary>
    /// <param name="animation"></param>
    public void playAnimation(List<AnimationFrame> animation)
    {
        if (anim != animation)
        {
            anim = animation;
            i = 0;
            currentFrame = 0;
            sprite.sprite = anim[0].Sprite;
        }
    }
}
