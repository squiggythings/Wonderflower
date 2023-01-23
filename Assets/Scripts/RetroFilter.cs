using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroFilter : MonoBehaviour
{
    public GameObject filter;

    // Update is called once per frame
    private void Start()
    {
        GameOptions.Setup();
        AudioManager.instance.setSFXVolume(GameOptions._SFXVOL / 10f);
#if UNITY_WEBGL

#else
    Screen.SetResolution(GameOptions.GetResolution().x, GameOptions.GetResolution().y, GameOptions._FULLSCREEN);
#endif
    }
    void Update()
    {
        filter.SetActive(GameOptions._FILTER);
    }
}
