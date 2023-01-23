using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public List<string> pageNoFile;
    public List<string> pageHasFile;
    public List<string> pageResetFile;
    public int subtractValue;
    public int baseHeight;
    bool hasFile;
    List<string> currentPageText;
    public TextMeshProUGUI header;
    public TextMeshProUGUI header2;
    public TextMeshProUGUI body;
    public TextMeshProUGUI body2;
    public SoundEffect blip;
    public SoundEffect startGame;
    public SoundEffect select;
    public GameObject screenCover;
    public InputManager input;
    public bool stopScripts;
    int titleIndex = 0;
    string titlePage = "main";
    public AudioSource titleMusic;
    public SoundEffect musicVolume;
    public float musicMixer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hasFile = PlayerPrefs.HasKey("u");
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(LoadGameplay());
        }
        if (!stopScripts)
        {
            if (input.GetButtonDown(Actions.Up))
            {
                if (titleIndex > 0)
                {
                    AudioManager.instance.playSoundEffect(blip);
                    --titleIndex;
                }

            }
            if (input.GetButtonDown(Actions.Down))
            {
                if (titleIndex < getCurrentPage().Count - 2)
                {
                    AudioManager.instance.playSoundEffect(blip);
                    ++titleIndex;
                }
            }
            if (input.GetButtonDown(Actions.Jump))
            {
                if (titlePage == "main")
                {
                    if (hasFile)
                    {
                        switch (titleIndex)
                        {
                            case 0:
                                // continue
                                StartCoroutine(LoadGameplay());
                                break;
                            case 1:
                                // new game warning
                                titlePage = "reset";
                                titleIndex = 0;
                                AudioManager.instance.playSoundEffect(select);
                                break;
                            case 2:
                                // quit
                                Application.Quit();
                                break;
                        }
                    }
                    else
                    {
                        switch (titleIndex)
                        {
                            case 0:
                                // continue
                                PlayerData temp = new PlayerData();
                                temp.saveToPlayerPrefs();
                                StartCoroutine(LoadGameplay());
                                break;
                            case 1:
                                // quit
                                Application.Quit();
                                break;
                        }
                    }
                }
                else
                {
                    switch (titleIndex)
                    {
                        case 0:
                            // start new game
                            PlayerData temp = new PlayerData();
                            temp.saveToPlayerPrefs();
                            StartCoroutine(LoadGameplay());
                            break;
                        case 1:
                            // go back
                            titlePage = "main";
                            titleIndex = 1;
                            AudioManager.instance.playSoundEffect(select);
                            break;
                    }
                }
            }
            if (input.GetButtonDown(Actions.Back) && titlePage == "reset")
            {
                titlePage = "main";
                titleIndex = 1;
                AudioManager.instance.playSoundEffect(select);
            }
            header.text = getCurrentPage()[0];
            header2.text = getCurrentPage()[0];

            body.text = formatStringFromPage(getCurrentPage(), titleIndex);
            body2.text = formatStringFromPage(getCurrentPage(), titleIndex);
            transform.localPosition = new Vector3(0, titlePage == "reset" ? baseHeight - subtractValue : baseHeight, 0);
            titleMusic.volume = GameOptions._MUSVOL / 10f * musicVolume.GetVolumeMultiplier() * musicMixer;
        }
    }

    public IEnumerator LoadGameplay()
    {
        titleMusic.Stop();
        stopScripts = true;
        AudioManager.instance.playSoundEffect(startGame);
        screenCover.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("gameplay");
    }

    string formatStringFromPage(List<string> pg, int idx)
    {
        string optionTack = "";
        string str = "";
        for (int i = 1; i < pg.Count; ++i)
        {
            if (i == idx + 1)
                str += "[ " + (pg[i] + optionTack).ToUpper() + " ]";
            else
                str += pg[i] + optionTack;
            str += "\n";
        }
        return str;
    }

    List<string> getCurrentPage()
    {
        switch (titlePage)
        {
            case "main":
                if (hasFile)
                {
                    return pageHasFile;
                }
                else
                {
                    return pageNoFile;
                }
            case "reset":
                return pageResetFile;
        }
        return null;
    }
}
