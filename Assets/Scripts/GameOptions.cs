using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions
{
    const int SCREEN_WIDTH = 224;
    const int SCREEN_HEIGHT = 176;
    public static void Setup()
    {
        if (!PlayerPrefs.HasKey("opt_sfx"))
        {
            PlayerPrefs.SetInt("opt_sfx", 8);
        }
        if (!PlayerPrefs.HasKey("opt_mus"))
        {
            PlayerPrefs.SetInt("opt_mus", 8);
        }
        if (!PlayerPrefs.HasKey("opt_scale"))
        {
            PlayerPrefs.SetInt("opt_scale", getMaximumScreenScale());
        }
        if (!PlayerPrefs.HasKey("opt_fullscreen"))
        {
            PlayerPrefs.SetInt("opt_fullscreen", 1);
        }
        if (!PlayerPrefs.HasKey("opt_scroll"))
        {
            PlayerPrefs.SetInt("opt_scroll", 0);
        }
        if (!PlayerPrefs.HasKey("opt_filter"))
        {
            PlayerPrefs.SetInt("opt_filter", 0);
        }
    }

    public static bool _FULLSCREEN => PlayerPrefs.GetInt("opt_fullscreen", 0) == 1;
    public static bool _FILTER => PlayerPrefs.GetInt("opt_filter", 0) == 1;
    public static bool _INSTANTSCROLL => PlayerPrefs.GetInt("opt_scroll", 0) == 1;
    public static int _SCREENSCALE => PlayerPrefs.GetInt("opt_scale", 4);
    public static int _SFXVOL => PlayerPrefs.GetInt("opt_sfx", 8);
    public static int _MUSVOL => PlayerPrefs.GetInt("opt_mus", 8);

    public static bool increaseSFXVolume()
    {
        bool ret = PlayerPrefs.GetInt("opt_sfx") < 10;
        if (ret)
            PlayerPrefs.SetInt("opt_sfx", PlayerPrefs.GetInt("opt_sfx") + 1);
        AudioManager.instance.setSFXVolume(PlayerPrefs.GetInt("opt_sfx") / 10f);
        PlayerPrefs.Save();
        return ret;
    }
    public static bool decreaseSFXVolume()
    {
        bool ret = PlayerPrefs.GetInt("opt_sfx") > 0;
        if (ret)
            PlayerPrefs.SetInt("opt_sfx", PlayerPrefs.GetInt("opt_sfx") - 1);
        AudioManager.instance.setSFXVolume(PlayerPrefs.GetInt("opt_sfx") / 10f);
        PlayerPrefs.Save();
        return ret;
    }

    static int getMaximumScreenScale()
    {
        for (int i = 1; i < 16; i++)
        {
            if (SCREEN_HEIGHT * i > Screen.currentResolution.height || SCREEN_WIDTH * i > Screen.currentResolution.width)
                return i - 1;
        }
        return 16;
    }

    public static Vector2Int GetResolution()
    {
        return new Vector2Int(SCREEN_WIDTH * _SCREENSCALE, SCREEN_HEIGHT * _SCREENSCALE);
    }
    public static void toggleFilter()
    {
        PlayerPrefs.SetInt("opt_filter", 1 - PlayerPrefs.GetInt("opt_filter"));
        PlayerPrefs.Save();
    }

    public static void toggleFullscreen()
    {
#if UNITY_WEBGL

#else
        PlayerPrefs.SetInt("opt_fullscreen", 1 - PlayerPrefs.GetInt("opt_fullscreen"));
        if (PlayerPrefs.GetInt("opt_fullscreen") == 1)
            PlayerPrefs.SetInt("opt_scale", getMaximumScreenScale());
        PlayerPrefs.Save();
    Screen.SetResolution(GetResolution().x, GetResolution().y, _FULLSCREEN);
#endif
    }

    public static void toggleScreenScroll()
    {
        PlayerPrefs.SetInt("opt_scroll", 1 - PlayerPrefs.GetInt("opt_scroll"));
        PlayerPrefs.Save();
    }

    public static void increaseScreenScale(int direction)
    {
#if UNITY_WEBGL

#else
        int a = PlayerPrefs.GetInt("opt_scale");
        a += direction;
        if (a < 2)
            a = getMaximumScreenScale();
        if (a > getMaximumScreenScale())
            a = 2;
        PlayerPrefs.SetInt("opt_scale", a);
        PlayerPrefs.SetInt("opt_fullscreen", 0);
        PlayerPrefs.Save();
    Screen.SetResolution(GetResolution().x, GetResolution().y, _FULLSCREEN);
#endif
    }

    public static bool increaseMusicVolume()
    {
        bool ret = PlayerPrefs.GetInt("opt_mus") < 10;
        if (ret)
            PlayerPrefs.SetInt("opt_mus", PlayerPrefs.GetInt("opt_mus") + 1);
        PlayerPrefs.Save();
        return ret;
    }
    public static bool decreaseMusicVolume()
    {
        bool ret = PlayerPrefs.GetInt("opt_mus") > 0;
        if (ret)
            PlayerPrefs.SetInt("opt_mus", PlayerPrefs.GetInt("opt_mus") - 1);
        PlayerPrefs.Save();
        return ret;
    }
}
