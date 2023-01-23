using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShift : MonoBehaviour
{
    public Material mat;
    public float hueShiftSpeed;
    public AudioSource shimmer;
    public float mixer = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shimmer.volume = mixer * GameOptions._SFXVOL / 10f * MusicManager.instance.volMultiplier;
        mat.SetVector("_HSLAAdjust", new Vector4(Time.unscaledTime * -hueShiftSpeed % 1, 0, 0, 0));
    }
}
