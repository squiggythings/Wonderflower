using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneParser : MonoBehaviour
{
    public string getRawCutscene(string csc)
    {
        string fullText = "{\n" + csc + "\n}";
        int start = 0;
        string cscText = "";

        int i = start;
        while (fullText[i] != '{')
            i++;
        start = i + 1;
        for (int j = start; j < fullText.Length; j++)
        {
            if (fullText[j] == '}')
                break;
            if (fullText[j] != '\n')
                cscText += fullText[j] + "";
            if (fullText[j] == '/' || fullText[j] == '+')
                cscText += "";
        }

        return cscText;
    }

}
