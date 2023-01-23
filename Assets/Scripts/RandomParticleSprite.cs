using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticleSprite : MonoBehaviour
{
    public SpriteRenderer sprite;
    public int framesUntilDecay;
    public List<Sprite> spriteVariations;
    private int i;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = spriteVariations[Random.Range(0, spriteVariations.Count)];
    }

    private void Update()
    {
        ++i;
        if (i > framesUntilDecay)
            Destroy(gameObject);
    }
}
