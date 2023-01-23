using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleDecay : MonoBehaviour
{
    public SpriteRenderer sprite;

    [SerializeField]
    private List<AnimationFrame> particleAnimation = new List<AnimationFrame>();
    private int i;
    private int currentFrame;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        i = 0;
        currentFrame = 0;
        sprite.sprite = particleAnimation[0].Sprite;
    }
    private void Update()
    {
        if (particleAnimation.Count > 1)
        {
            if (i >= particleAnimation[currentFrame].FrameLength)
            {
                i = 0;
                ++currentFrame;
                if (currentFrame >= particleAnimation.Count)
                    Destroy(gameObject);
                else
                    sprite.sprite = particleAnimation[currentFrame].Sprite;
            }
            ++i;
        }
    }
}
