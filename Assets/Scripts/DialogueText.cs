using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueText : MonoBehaviour
{

    public List<int> shakeEffectIndexes = new List<int>();
    public List<int> waveEffectIndexes = new List<int>();
    public List<int> highlightEffectIndexes = new List<int>();

    private const float LINE_HEIGHT_REGULAR = -13.1f;
    private const float LINE_HEIGHT_NOSPEAKER = 5.71f;
    private const float MASK_POSITION_REGULAR = -5.5f;
    private const float MASK_POSITION_NOSPEAKER = 2.5f;
    [SerializeField]
    private Transform maskPosition;

    [SerializeField]
    private TextMeshProUGUI mainText;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private Color highlightColor;
    Mesh mesh;
    Vector3[] vertices;
    Color[] colors;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateEffects();
    }

    void updateEffects()
    {
        mainText.ForceMeshUpdate();
        mesh = mainText.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
        for (int i = 0; i < mainText.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = mainText.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            if (!char.IsWhiteSpace(c.character))
            {
                if (shakeEffectIndexes.Contains(i))
                {
                    Vector3 offset = getShakeEffect();
                    for (int j = 0; j < 4; j++)
                    {
                        vertices[index + j] += offset;
                    }
                }
                if (waveEffectIndexes.Contains(i))
                {
                    Vector3 offset = getWaveEffect(i);
                    for (int j = 0; j < 4; j++)
                    {
                        vertices[index + j] += offset;
                    }
                }
                if (highlightEffectIndexes.Contains(i))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        colors[index + j] = highlightColor;
                    }
                }
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mainText.canvasRenderer.SetMesh(mesh);
    }

    public void moveViewUpOneLine(int speed)
    { StartCoroutine(moveViewUp(speed)); }

    public void clearTextAndReset()
    {
        shakeEffectIndexes.Clear();
        waveEffectIndexes.Clear();
        highlightEffectIndexes.Clear();
        mainText.text = " ";
        mainText.ForceMeshUpdate();
        mainText.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void setTextToActuallyNothing()
    {
        mainText.text = "";
        updateEffects();
    }

    public void addCharacter(char c, char effect)
    {
        mainText.text += "" + c;
        switch (effect)
        {
            case 's':
                shakeEffectIndexes.Add(mainText.text.Length - 1);
                break;
            case 'w':
                waveEffectIndexes.Add(mainText.text.Length - 1);
                break;
            case '#':
                highlightEffectIndexes.Add(mainText.text.Length - 1);
                break;
        }
        updateEffects();
    }

    public void newLine()
    {
        mainText.text += "\n";
    }

    public void setSpeakingName(string name)
    {
        if (name == "playerNoName" || name == "#")
        {
            characterName.text = "";
            mainText.lineSpacing = LINE_HEIGHT_NOSPEAKER;
            maskPosition.localPosition = new Vector3(0, MASK_POSITION_NOSPEAKER, 0);
        }
        else
        {
            characterName.text = name;
            mainText.lineSpacing = LINE_HEIGHT_REGULAR;
            maskPosition.localPosition = new Vector3(0, MASK_POSITION_REGULAR, 0);
        }
    }

    IEnumerator moveViewUp(int speed)
    {
        int animationLength = speed;
        int lineHeight = 13;
        for (int i = 0; i < animationLength; i++)
        {
            mainText.gameObject.transform.localPosition += Vector3.up * lineHeight / animationLength;
            yield return null;
        }
        mainText.gameObject.transform.localPosition = new Vector3(0, 13, 0);
        string[] lines = mainText.text.Split('\n');
        mainText.text = "";
        foreach (char c in lines[0])
        {
            mainText.text += " ";
        }
        if (lines.Length > 2)
            mainText.text += "\n" + lines[1] + "\n" + lines[2];
    }

    Vector3 getShakeEffect()
    {
        float amt = 1;
        float quantize = 1;
        return new Vector3(quantizeNum(Random.Range(-amt, amt), quantize), quantizeNum(Random.Range(-amt, amt), quantize), 0);
    }

    Vector3 getWaveEffect(int i)
    {
        float quantize = 1;
        float depth = 1;
        float speed = 12;
        float rippleWidth = 0.6667f;
        return new Vector3(0, quantizeNum(Mathf.Sin(Time.time * speed + i * rippleWidth) * depth, quantize), 0);
    }

    float quantizeNum(float num, float depth)
    {
        return Mathf.Round(num / depth) * depth;
    }
}

