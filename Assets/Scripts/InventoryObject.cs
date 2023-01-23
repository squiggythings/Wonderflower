using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryObject", order = 2)]
public class InventoryObject : ScriptableObject
{
    [Tooltip("The name that will display in the inventory")]
    public string displayName;
    [Tooltip("The description text for this object")]
    [TextArea]
    public string description;
    [Tooltip("The graphic of this sprite in the inventory")]
    public Sprite image;
    public ObjectType type;

    public string getType()
    {
        if (type == ObjectType.KeyItem) return "KeyItem";
        if (type == ObjectType.Powerup) return "Powerup";
        if (type == ObjectType.Regular) return "Regular";
        return null;
    }

    public enum ObjectType
    {
        Regular,
        KeyItem,
        Powerup,
    }
}
