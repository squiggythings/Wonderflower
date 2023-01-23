using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditAnimation : MonoBehaviour
{
    public string animName;
    public int duration;
    float start, end;
    bool hasShown;
    bool hasHidden;
    public bool ending;
    SpriteRenderer sprite;
    public List<AnimationLoop> animLoop;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        start = -transform.localPosition.y;
        end = start + duration;
        transform.position = new Vector3(transform.position.x, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CreditScroll.POSITION >= start && !hasShown)
        {
            StartCoroutine(slide(-1));
            StartCoroutine(playAnimation(animLoop));
            hasShown = true;
        }

        if (CreditScroll.POSITION >= end && !hasHidden)
        {
            StartCoroutine(slide(1));
            hasHidden = true;
        }
    }
    public IEnumerator playAnimation(List<AnimationLoop> anim)
    {
        for (int i = 0; i < 6; ++i)
        {
            yield return null;
        }
        yield return null;
        foreach (AnimationLoop loop in anim)
        {
            for (int i = 0; i < loop.timesToLoop; ++i)
            {
                foreach (AnimationFrame frame in loop.animation)
                {
                    sprite.sprite = frame.Sprite;
                    for (int j = 0; j < frame.FrameLength; ++j) {
                        yield return null;
                    }
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(transform.position + Vector3.left * 220, new Vector3(96 / 4, 4, 1));
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(transform.position + Vector3.left * 220 + Vector3.down * duration, new Vector3(96 / 4, 4, 1));
    }

    IEnumerator slide(int direction)
    {
        float distance = 96;
        int length = 6;
        if (ending)
        {
            distance = 112;
            length = 18;
            direction *= -1;
        }
        for (int i = 0; i < length; ++i)
        {
            transform.Translate(distance / length * direction, 0, 0);
            yield return null;
        }
    }
}

[System.Serializable]
public class AnimationLoop
{
    public int timesToLoop;
    public List<AnimationFrame> animation;
}
