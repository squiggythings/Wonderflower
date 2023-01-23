using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimatorTyper : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField]
    private int slowTypeSpeed;
    [SerializeField]
    private int fastTypeSpeed;
    [SerializeField]
    private int fastTypeSoundBuffer;
    [SerializeField]
    private int animationMoveUpSpeed;
    public bool isTyping;

    [SerializeField]
    private bool showTextBox;

    [Header("References")]
    [SerializeField]
    private DialogueText textBox;
    [SerializeField]
    private SpriteRenderer inputIndicator;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private CamFollow cam;
    [SerializeField]
    private Transform backgroundBox;
    [SerializeField]
    private Transform textBubblePointer;
    private bool instantText;

    private string currentSpeaker;

    [Header("Sounds")]
    [SerializeField]
    private SoundEffect type;
    [SerializeField]
    private SoundEffect nextPage;
    [SerializeField]
    private SoundEffect endText;
    public bool canSpeedUpText = true;

    // Start is called before the first frame update
    void Start()
    {
        textBox = FindObjectOfType<DialogueText>();
        player = FindObjectOfType<PlayerController>();
        cam = FindObjectOfType<CamFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.input.GetButtonDown(Actions.Back) && !player.input.GetButton(Actions.Down) && canSpeedUpText)
            instantText = true;
        if (player.input.GetButtonUp(Actions.Back) || !canSpeedUpText)
            instantText = false;
        if (showTextBox)
            positionPointer(currentSpeaker);
    }

    public bool isNPCtalking()
    {
        return showTextBox && (currentSpeaker != "SHERBET" && currentSpeaker != "#");
    }

    public void writeText(string txt)
    {
        textBox.clearTextAndReset();
        StartCoroutine(write(txt));
    }

    public void SetSpeaker(string name)
    {
        currentSpeaker = name;
        textBox.setSpeakingName(name);
        StartCoroutine(_bob());
    }

    public IEnumerator _bob()
    {
        AudioManager.instance.playSoundEffect(nextPage);
        float bob_y = -6;
        float spd = 6;
        for (int i = 0; i < spd; i++)
        {
            positionElements((int)bob_y);
            bob_y += 6f / spd;
            yield return null;
        }
        positionElements(0);
    }

    public void showBox()
    {
        showTextBox = true;
        // StartCoroutine(_bob());
    }

    public void hideBox()
    {
        textBox.clearTextAndReset();
        AudioManager.instance.playSoundEffect(endText);
        showTextBox = false;
        positionElements(0);
    }

    void positionElements(float offset)
    {
        float baseY = player.transform.position.y + (showTextBox ? 0 : 999) + offset - cam.transform.position.y;
        backgroundBox.localPosition = new Vector3(0, baseY + 62, 0);
        textBubblePointer.localScale = new Vector3(1, -1, 1);
        if (backgroundBox.localPosition.y > 58)
        {
            backgroundBox.localPosition = new Vector3(0, baseY - 38, 0);
            textBubblePointer.localScale = new Vector3(1, 1, 1);
        }
        positionPointer(currentSpeaker);
    }

    void positionPointer(string name)
    {
        if (name == "#")
        {
            // effectively hide the pointer moving it far offscreen
            textBubblePointer.localPosition = new Vector3(0, 999, 0);
            backgroundBox.localPosition = new Vector3(0, showTextBox ? 0 : 999, 0);
        }
        else if (name == "SHERBET" || name == "playerNoName")
            textBubblePointer.localPosition = new Vector3(cam.playerScreenPosition().x, 0, 0);
        else
            textBubblePointer.localPosition = new Vector3(CutscenePlayer.NPCPosition.x - cam.transform.position.x, 0, 0);
    }

    public IEnumerator write(string text)
    {
        textBox.setTextToActuallyNothing();
        isTyping = true;
        yield return null;
        int line = 1;
        char effect = 'n';
        for (int i = 0; i < text.Length; ++i)
        {
            // special escape characters
            if (text[i] == '/')
            {
                if (instantText)
                    AudioManager.instance.playSoundEffect(type);
                yield return null;
                while (!player.input.GetButtonDown(Actions.Jump))
                    yield return null;
                i += 1;
                line = 1;
                effect = 'n';
                textBox.clearTextAndReset();
                textBox.setTextToActuallyNothing();
            }

            if (i >= text.Length)
                break;

            if (text[i] == '+')
            {
                if (line > 1)
                {
                    if (instantText)
                        AudioManager.instance.playSoundEffect(type);
                    yield return null;
                    while (!player.input.GetButtonDown(Actions.Jump))
                        yield return null;
                    textBox.moveViewUpOneLine(animationMoveUpSpeed + 2);
                    for (int k = 0; k < animationMoveUpSpeed; ++k)
                        yield return null;
                }
                ++line;
                textBox.newLine();
                i += 1;
            }
            if (text[i] == '[')
            {
                effect = text[i + 1];
                i += 3;
            }

            if (i >= text.Length)
                break;

            // type character
            textBox.addCharacter(text[i], effect);

            // sounds and waiting
            if (!instantText)
            {
                if (player.input.GetButton(Actions.Jump))
                {
                    for (int k = 0; k < fastTypeSpeed; ++k)
                        yield return null;
                    if (fastTypeSoundBuffer == 0 || Time.frameCount % fastTypeSoundBuffer == 0)
                        AudioManager.instance.playSoundEffect(type);
                }
                else
                {
                    for (int k = 0; k < slowTypeSpeed; ++k)
                        yield return null;
                    AudioManager.instance.playSoundEffect(type);
                }
            }
        }
        isTyping = false;
    }

}
