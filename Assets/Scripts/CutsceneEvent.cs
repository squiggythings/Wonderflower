using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneEvent
{
    [Tooltip("Purely for readability purposes. Serves no use otherwise.")]
    [SerializeField]
    private string cutsceneName;
    [Tooltip("Leave blank for no requirement")]
    public InventoryObject[] InventoryMustContain;
    [Tooltip("Leave blank for no requirement")]
    public string[] CutsceneMustContain;
    [Tooltip("Custom cutscene script")]
    public CSCScript script;

    public bool meetsRequirements(PlayerData playerData)
    {
        bool inv;
        bool csc;
        if (InventoryMustContain.Length == 0)
            inv = true;
        else
        {
            inv = true;
            foreach (InventoryObject s in InventoryMustContain)
            {
                if (!playerData.inventoryContains(s.name)) inv = false;
            }
        }
        if (CutsceneMustContain.Length == 0)
            csc = true;
        else
        {
            csc = true;
            foreach (string s in CutsceneMustContain)
            {
                if (!playerData.cutscene.Contains(s)) csc = false;
            }
        }
        return inv && csc;
    }

    public string getFlagID => "0";
}

[System.Serializable]
public class CSCScript
{
    [TextArea(minLines:1,maxLines:99)]
    public string data;
}