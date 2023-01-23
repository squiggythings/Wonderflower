using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObjectList : MonoBehaviour
{
    private Object[] loadFromResources;
    public static Dictionary<string, InventoryObject> objects = new Dictionary<string, InventoryObject>();
    void Awake()
    {
        loadFromResources = Resources.LoadAll("InventoryObjects", typeof(InventoryObject));
        foreach (InventoryObject s in loadFromResources)
        {
            if (!objects.ContainsKey(s.name.ToUpper()))
                objects.Add(s.name.ToUpper(), s);
        }
    }


}
