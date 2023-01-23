using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int positionX;
    public int positionY;
    public List<InventoryObject> inventory;
    public List<string> cutscene;
    public List<InventoryObject> powerups;
    public List<Vector3Int> unvisitedScreens;
    public PlayerData()
    {
        positionX = 890;
        positionY = -760;
        inventory = new List<InventoryObject>();
        powerups = new List<InventoryObject>();
        cutscene = new List<string>();
        ResetScreens();
    }

    public void loadFromPlayerPrefs()
    {
        cutscene.Add("firstCSC");
        positionX = int.Parse(TextEncoder.textDecode(PlayerPrefs.GetString("u", "606162")));
        positionY = int.Parse(TextEncoder.textDecode(PlayerPrefs.GetString("v", "76595862")));
        cutscene = TextEncoder.expandList(TextEncoder.textDecode(PlayerPrefs.GetString("c")), '%');
        inventory = stringsToInventoryObject(TextEncoder.expandList(TextEncoder.textDecode(PlayerPrefs.GetString("i")), '%'));
        // #CHEATS
        /*
        inventory.Clear();
        addToInventory("BUCKET");
        cutscene.Add("powerup.gloves");
        addToInventory("FERTILIZER");
        addToInventory("WATERING CAN");
        */
        // #/CHEATS

        if (cutscene.Contains("powerup.gloves"))
            addToPowerups("powerup.gloves");
        if (cutscene.Contains("powerup.boots"))
            addToPowerups("powerup.boots");
        if (cutscene.Contains("powerup.feather"))
            addToPowerups("powerup.feather");
        string scr = PlayerPrefs.GetString("s", "poopyfarts");
        if (scr == "poopyfarts")
        {
            ResetScreens();
        }
        else
        {
            unvisitedScreens.Clear();
            for (int i = 0; i < scr.Length; i += 2)
            {
                unvisitedScreens.Add(new Vector3Int(int.Parse(scr[i] + ""), -int.Parse(scr[i + 1] + ""), 0));
            }
        }
    }

    public void saveToPlayerPrefs()
    {
        PlayerPrefs.SetString("u", TextEncoder.textEncode("" + positionX));
        PlayerPrefs.SetString("v", TextEncoder.textEncode("" + positionY));
        string cutsceneData = TextEncoder.condenseList(cutscene, '%');
        string inventoryData = TextEncoder.condenseList(inventoryObjToStrings(inventory), '%');
        string unvisitedScreenData = "";
        foreach (Vector3Int v in unvisitedScreens)
        {
            unvisitedScreenData += v.x + "" + Mathf.Abs(v.y);
        }


        PlayerPrefs.SetString("c", TextEncoder.textEncode(cutsceneData));
        PlayerPrefs.SetString("i", TextEncoder.textEncode(inventoryData));
        PlayerPrefs.SetString("s", unvisitedScreenData);
        PlayerPrefs.Save();
    }

    public void ResetScreens()
    {
        unvisitedScreens = new List<Vector3Int>();
        unvisitedScreens.Clear();
        for (int y = 0; y > -7; --y)
        {
            for (int x = 0; x < 8; ++x)
            {
                unvisitedScreens.Add(new Vector3Int(x, y, 0));
            }
        }
    }

    List<string> inventoryObjToStrings(List<InventoryObject> inv)
    {
        List<string> ret = new List<string>();
        foreach (InventoryObject obj in inv)
        {
            ret.Add(obj.name.ToUpper());
        }
        return ret;
    }
    List<InventoryObject> stringsToInventoryObject(List<string> str)
    {
        List<InventoryObject> ret = new List<InventoryObject>();
        foreach (string obj in str)
        {
            ret.Add(InventoryObjectList.objects[obj.ToUpper()]);
        }
        return ret;
    }

    public bool inventoryContains(string obj)
    {
        return inventory.Contains(InventoryObjectList.objects[obj.ToUpper()]);
    }

    public void addToInventory(string obj)
    {
        inventory.Add(InventoryObjectList.objects[obj.ToUpper()]);
    }

    public void removeFromInventory(string obj)
    {
        inventory.Remove(InventoryObjectList.objects[obj.ToUpper()]);
    }

    public void replaceInInventory(string objToReplace, string replacingWith)
    {
        inventory.Insert(inventory.IndexOf(InventoryObjectList.objects[objToReplace.ToUpper()]), InventoryObjectList.objects[replacingWith.ToUpper()]);
        removeFromInventory(objToReplace.ToUpper());
    }

    public void addToPowerups(string obj)
    {
        powerups.Add(InventoryObjectList.objects[obj.ToUpper()]);
    }

    public void visitScreen(Vector3Int screenPos)
    {
        if (unvisitedScreens.Contains((screenPos)))
        {
            unvisitedScreens.Remove((screenPos));
        }
    }
}

[System.Serializable]
public class ScreenPosition
{
    [SerializeField]
    int x;
    [SerializeField]
    int y;

    public ScreenPosition(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public string getString()
    {
        return "" + x + y;
    }

    public static ScreenPosition convertFromV3(Vector3Int v3)
    {
        return new ScreenPosition(v3.x, v3.y);
    }

    public Vector3Int getVector3()
    {
        return new Vector3Int(x, y, 0);
    }
}