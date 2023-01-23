using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEncoder : MonoBehaviour
{
    public static string textEncode(string txt)
    {
        string alphabet = '\t' + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+=-`~,./?><;': " + '"' + '\n' + '\\';
        string retValue = "";
        int n = 0, i = 0;
        while (n < txt.Length)
        {
            i = alphabet.IndexOf(txt[n]);
            if (i < 10)
            {
                retValue += "0" + i;
            }
            else
            {
                retValue += "" + i;
            }
            n++;
        }
        return retValue;
    }

    public static string textDecode(string num)
    {
        string alphabet = '\t' + "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+=-`~,./?><;': " + '"' + '\n' + '\\';
        string retValue = "";
        int n = 0;
        while (n < num.Length)
        {
            retValue += alphabet[int.Parse(num[n] + "" + num[n + 1])];
            n += 2;
        }
        return retValue;
    }

    public static string condenseList(List<string> list, char separator)
    {
        string retValue = "";
        foreach (string str in list)
        {
            retValue += str + separator;
        }
        return retValue;
    }

    public static List<string> expandList(string str, char separator)
    {
        List<string> ret = new List<string>();
        string item = "";
        foreach (char ch in str)
        {
            if (ch == separator)
            {
                ret.Add(item);
                item = "";
            }
            else
            {
                item += ch;
            }
        }
        return ret;
    }
}
