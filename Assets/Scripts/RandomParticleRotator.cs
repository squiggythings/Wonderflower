using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticleRotator : MonoBehaviour
{
    [Tooltip("Randomly rotate any multiple of 90 degrees")]
    public bool rotateAngles;
    [Tooltip("Have a chance to flip on the x axis?")]
    public bool flipX;
    [Tooltip("Have a chance to flip on the y axis?")]
    public bool flipY;
    void Start()
    {
        int r = Random.Range(0, 4);
        r *= 90;
        if (rotateAngles)
            transform.Rotate(new Vector3(0, 0, 1) * r);
        if (flipX)
            GetComponent<SpriteRenderer>().flipX = Random.Range(0, 1f) < 0.5f;
        if (flipY)
            GetComponent<SpriteRenderer>().flipY = Random.Range(0, 1f) < 0.5f;
    }
}
