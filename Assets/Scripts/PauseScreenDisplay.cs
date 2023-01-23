using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseScreenDisplay : MonoBehaviour
{
    public InventoryDisplay inv;
    public GameObject container;
    private PlayerController player;
    public TextMeshProUGUI header;
    public TextMeshProUGUI body;
    public SoundEffect pause;
    public SoundEffect unpause;
    public SoundEffect blip;
    public static bool isPaused;
    int pauseIndex;
    Page pausePage;
    public List<string> pageMain;
    public List<string> pageOptions;
    List<string> currentPageText;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CamFollow.isMoving && !CutscenePlayer.isInCutscene)
        {
            if (player.input.GetButtonDown(Actions.Pause) && !InventoryDisplay.isInventoryOpen)
                activatePause();
            if (player.input.GetButtonDown(Actions.Inventory) && !isPaused)
                inv.activateInventory();
        }
        container.SetActive(isPaused);
        if (isPaused)
        {
            if (player.input.GetButtonDown(Actions.Jump))
            {
                if (pausePage == Page.main)
                    switch (pauseIndex)
                    {
                        case 0:
                            activatePause();
                            break;
                        case 1:
                            pausePage = Page.options;
                            pauseIndex = 0;
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                        case 2:
#if UNITY_WEBGL
#else
                            Application.Quit();
#endif
                            break;
                    }
                else if (pausePage == Page.options)
                {
                    switch (pauseIndex)
                    {
                        case 2:
                            GameOptions.increaseScreenScale(1);
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 3:
                            GameOptions.toggleFullscreen();
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 4:
                            GameOptions.toggleScreenScroll();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                        case 5:
                            GameOptions.toggleFilter();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                        case 6:
                            pausePage = Page.main;
                            pauseIndex = 1;
                            AudioManager.instance.playSoundEffect(unpause);
                            break;
                    }
                }
            }
            if (pausePage == Page.options)
            {
                if (player.input.GetButtonDown(Actions.Right))
                {
                    switch (pauseIndex)
                    {
                        case 0:
                            if (GameOptions.increaseSFXVolume())
                                AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 1:
                            if (GameOptions.increaseMusicVolume())
                                AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 2:
                            GameOptions.increaseScreenScale(1);
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 3:
                            GameOptions.toggleFullscreen();
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 4:
                            GameOptions.toggleScreenScroll();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                        case 5:
                            GameOptions.toggleFilter();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                    }
                }
                if (player.input.GetButtonDown(Actions.Back))
                {
                    pausePage = Page.main;
                    pauseIndex = 1;
                    AudioManager.instance.playSoundEffect(unpause);
                }
                if (player.input.GetButtonDown(Actions.Left))
                {
                    switch (pauseIndex)
                    {
                        case 0:
                            if (GameOptions.decreaseSFXVolume())
                                AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 1:
                            if (GameOptions.decreaseMusicVolume())
                                AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 2:
                            GameOptions.increaseScreenScale(-1);
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 3:
                            GameOptions.toggleFullscreen();
                            AudioManager.instance.playSoundEffect(blip);
                            break;
                        case 4:
                            GameOptions.toggleScreenScroll();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                        case 5:
                            GameOptions.toggleFilter();
                            AudioManager.instance.playSoundEffect(pause);
                            break;
                    }
                }
            }
            if (player.input.GetButtonDown(Actions.Up))
            {
                if (pauseIndex > 0)
                {
                    AudioManager.instance.playSoundEffect(blip);
                    --pauseIndex;
                }

            }
            if (player.input.GetButtonDown(Actions.Down))
            {
                if (pauseIndex < getCurrentPage(pausePage).Count - 2)
                {
                    AudioManager.instance.playSoundEffect(blip);
                    ++pauseIndex;
                }
            }
        }

        header.text = getCurrentPage(pausePage)[0];

        body.text = formatStringFromPage(getCurrentPage(pausePage), pauseIndex);
        container.transform.localPosition = new Vector3(0, pausePage == Page.options ? 0 : -18, 0);
    }

    string formatStringFromPage(List<string> pg, int idx)
    {
        string optionTack = "";
        string str = "";
        for (int i = 1; i < pg.Count; ++i)
        {
            if (pg[0] == pageOptions[0])
            {
                switch (i)
                {
                    case 1:
                        optionTack = ": " + GameOptions._SFXVOL;
                        break;
                    case 2:
                        optionTack = ": " + GameOptions._MUSVOL;
                        break;
                    case 3:
                        optionTack = ": " + GameOptions._SCREENSCALE;
                        break;
                    case 4:
                        optionTack = ": " + (GameOptions._FULLSCREEN ? "on" : "off");
                        break;
                    case 5:
                        optionTack = ": " + (GameOptions._INSTANTSCROLL ? "on" : "off");
                        break;
                    case 6:
                        optionTack = ": " + (GameOptions._FILTER ? "on" : "off");
                        break;
                    case 7:
                        optionTack = "";
                        break;
                }
            }
            if (i == idx + 1)
                str += "[ " + (pg[i] + optionTack).ToUpper() + " ]";
            else
                str += pg[i] + optionTack;
            str += "\n";
        }
        return str;
    }
    List<string> getCurrentPage(Page p)
    {
        switch (p)
        {
            case Page.main:
                return pageMain;
            case Page.options:
                return pageOptions;
        }
        return null;
    }
    void activatePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            player.pauseInput = true;
            player.pausePhysics = true;
            AudioManager.instance.playSoundEffect(pause);
            pauseIndex = 0;
            pausePage = Page.main;
        }
        else
        {
            player.pausePhysics = false;
            StartCoroutine(waitUntilReleaseZ());
            AudioManager.instance.playSoundEffect(unpause);
        }
    }
    enum Page { main, options, }

    IEnumerator waitUntilReleaseZ()
    {
        while (player.input.GetButton(Actions.Jump))
        {
            yield return null;
        }
        player.pauseInput = false;
    }
}
