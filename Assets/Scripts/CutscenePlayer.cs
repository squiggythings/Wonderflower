using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    public static Vector3 NPCPosition;
    public static string NPCName;
    public static bool isNPCTalking;
    public CutsceneParser parser;
    public DialogueAnimatorTyper textManager;
    public PlayerController player;

    public CinematicBars cinematicBars;
    public SoundEffect itemGet;
    public SoundEffect powerupGet;
    public SoundEffect openDoor;
    public static bool isInCutscene;
    public SpriteRenderer fade;
    public SpriteRenderer instructions;
    public GameObject wiltedFlower;
    public GameObject bucket;
    public GameObject wonderFlower;
    public SpriteRenderer whiteFlash;
    public SpriteRenderer wonderFlowerLogo;
    public SoundEffect shake;
    public SoundEffect burst;
    bool begin;
    void Start()
    {
        cinematicBars = FindObjectOfType<CinematicBars>();
        player = FindObjectOfType<PlayerController>();
        parser = GetComponent<CutsceneParser>();

    }

    // Update is called once per frame
    void Update()
    {
        isNPCTalking = textManager.isNPCtalking();
        if (player.playerData != null && !begin)
        {
            if (player.playerData.cutscene.Contains("firstCSC"))
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
            }
            begin = true;
        }
    }

    public void StartCutscene(CutsceneEvent cscEvent, NPCDialogue origin)
    {
        NPCPosition = origin.gameObject.transform.position;
        NPCName = origin.NPCName;
        StartCoroutine(playCutscene(parser.getRawCutscene(cscEvent.script.data)));
    }

    public void StartCutscene(CutsceneEvent cscEvent)
    {
        StartCoroutine(playCutscene(parser.getRawCutscene(cscEvent.script.data)));
    }

    IEnumerator playCutscene(string cscText)
    {
        bool skipSave = false;
        isInCutscene = true;
        string cmd;
        string txt;
        for (int j = 0; j < cscText.Length; ++j)
        {
            char c = cscText[j];
            if (c == '>')
            {
                txt = stringUntilChar('<', cscText, j);
                if (txt.Length > 2)
                {
                    textManager.writeText(txt);
                    while (textManager.isTyping)
                        yield return null;
                }
            }

            if (cscText[j] == '<')
            {
                cmd = stringUntilChar('>', cscText, j);
                if (cmd.Substring(0, 4) == "+csc")
                    player.playerData.cutscene.Add(cmd.Substring(cmd.IndexOf(':') + 1));
                else if (cmd.Substring(0, 4) == "+inv")
                    player.playerData.addToInventory(cmd.Substring(cmd.IndexOf(':') + 1));
                else if (cmd.Substring(0, 4) == "+pwr")
                    player.playerData.addToPowerups(cmd.Substring(cmd.IndexOf(':') + 1));
                else if (cmd.Substring(0, 4) == "-inv")
                    player.playerData.removeFromInventory(cmd.Substring(cmd.IndexOf(':') + 1));
                else if (cmd.Substring(0, 4) == "wait")
                    yield return new WaitForSeconds(float.Parse(cmd.Substring(cmd.IndexOf(':') + 1)));
                else if (cmd.Substring(0, 4) == "#sng")
                    MusicManager.instance.switchSong(0, cmd.Substring(cmd.IndexOf(':') + 1), 0);
                else if (cmd.Substring(0, 4) == "#sFd")
                    MusicManager.instance.switchSong(70, cmd.Substring(cmd.IndexOf(':') + 1), 0);
                else if (cmd.Substring(0, 4) == "#mus")
                    MusicManager.instance.switchSong(20, cmd.Substring(cmd.IndexOf(':') + 1), 50);
                else if (cmd.Substring(0, 4) == "#env")
                    player.cam.setEnvironment(cmd.Substring(cmd.IndexOf(':') + 1));
                yield return null;

                switch (cmd)
                {
                    case "dontSaveAfterEnd":
                        skipSave = true;
                        break;
                    case "#start":
                        player.pauseInput = true;
                        player.StartCutsceneMoveBack();
                        textManager.showBox();
                        cinematicBars.Show();
                        break;
                    case "#startNoMove":
                        player.pauseInput = true;
                        textManager.showBox();
                        break;
                    case "#startEnding":
                        player.pauseInput = true;
                        textManager.showBox();
                        break;
                    case "#hideBars":
                        cinematicBars.Hide();
                        break;
                    case "#showBars":
                        cinematicBars.Show();
                        break;
                    case "#startMoveLittle":
                        player.pauseInput = true;
                        player.StartCutsceneMoveOnlyALittle();
                        textManager.showBox();
                        cinematicBars.Show();
                        break;
                    case "#end":
                        textManager.hideBox();
                        cinematicBars.Hide();
                        while (!player.input.GetButtonUp(Actions.Jump))
                            yield return null;
                        player.pauseInput = false;
                        break;
                    case "#hideBox":
                        textManager.hideBox();
                        break;
                    case "#disableSpeedUp":
                        textManager.canSpeedUpText = false;
                        break;
                    case "#enableSpeedUp":
                        textManager.canSpeedUpText = true;
                        break;
                    case "#fadeIn":
                        for (int i = 60; i > 0; --i)
                        {
                            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, i / 60f);
                            yield return null;
                        }
                        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
                        break;
                    case "#fadeOut":
                        for (int i = 0; i < 120; ++i)
                        {
                            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, i / 120f);
                            yield return null;
                        }
                        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
                        break;
                    case "#pausePhysics":
                        player.pausePhysics = true;
                        break;
                    case "#resumePhysics":
                        player.pausePhysics = false;
                        break;
                    case "speaker=npc":
                        textManager.SetSpeaker(NPCName);
                        break;
                    case "speaker=#":
                        textManager.SetSpeaker("#");
                        break;
                    case "speaker=player":
                        textManager.SetSpeaker("SHERBET");
                        break;
                    case "speaker=playerNoName":
                        textManager.SetSpeaker("playerNoName");
                        break;
                    case "sfx:item":
                        AudioManager.instance.playSoundEffect(itemGet);
                        break;
                    case "sfx:door":
                        AudioManager.instance.playSoundEffect(openDoor);
                        break;
                    case "sfx:powerup":
                        AudioManager.instance.playSoundEffect(powerupGet);
                        MusicManager.instance.dipVolume(powerupGet);
                        break;
                    case "playerDir:-1":
                        player.direction = -1;
                        break;
                    case "playerDir:1":
                        player.direction = 1;
                        break;
                    case "playerMove:-1":
                        player.MoveInCutscene(24);
                        break;
                    case "playerMove:1":
                        player.MoveInCutscene(-24);
                        break;
                    case "itm:key":
                        if (player.playerData.inventoryContains("GOLD KEY"))
                        {
                            player.playerData.replaceInInventory("GOLD KEY", "GOLD KEYS");
                        }
                        else
                            player.playerData.addToInventory("GOLD KEY");
                        break;
                    case "showInstructions":
                        instructions.enabled = true;
                        break;
                    case "flash":
                        StartCoroutine(flash());
                        break;
                    case "showBucket":
                        AudioManager.instance.playSoundEffect(openDoor);
                        bucket.SetActive(true);
                        break;
                    case "showWonderflower":
                        StartCoroutine(flash());
                        FindObjectOfType<CamFollow>().showWonderFlower = true;
                        whiteFlash.color = new Color(1, 1, 1, 1);
                        AudioManager.instance.playSoundEffect(burst);
                        wonderFlower.SetActive(true);
                        bucket.SetActive(false);
                        wiltedFlower.SetActive(false);
                        for (int i = 90; i > 0; --i)
                        {
                            whiteFlash.color = new Color(1, 1, 1, i / 90f);
                            yield return null;
                        }
                        whiteFlash.color = new Color(1, 1, 1, 0);
                        break;
                    case "endingFade":
                        MusicManager.instance.switchSong(322, "silence", 320);
                        for (int i = 0; i < 320; ++i)
                        {
                            wonderFlowerLogo.color = new Color(1, 1, 1, i / 210f);
                            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, i / 320f);
                            yield return null;
                        }
                        wonderFlower.SetActive(false);
                        break;
                    case "#EVENT:toCredits":
                        SceneManager.LoadScene("credits");
                        break;
                }
            }

        }
        isInCutscene = false;
        if (!skipSave)
            player.playerData.saveToPlayerPrefs();
    }

    IEnumerator flash()
    {
        AudioManager.instance.playSoundEffect(shake);
        CamFollow cam = FindObjectOfType<CamFollow>();
        for (int i = 8; i > 0; i--)
        {
            whiteFlash.color = new Color(1, 1, 1, i / 30f);
            cam.ShakePosition(Random.Range(-1, 1), Random.Range(-1, 1));
            yield return null;
        }
        whiteFlash.color = new Color(1, 1, 1, 0);
        cam.ShakePosition(0, 0);
    }
    string stringUntilChar(char ch, string txt, int start)
    {
        int i = start + 1;
        string str = "";
        while (i < txt.Length - 1 && txt[i] != ch)
        {
            str += txt[i];
            i++;
        }
        return str;
    }
}
