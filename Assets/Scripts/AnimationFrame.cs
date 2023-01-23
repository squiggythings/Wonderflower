using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationFrame
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int frameLength = 5;

    public Sprite Sprite => sprite;
    public int FrameLength => frameLength;
}
