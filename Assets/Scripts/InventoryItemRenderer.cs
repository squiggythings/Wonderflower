using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemRenderer : MonoBehaviour
{
    [SerializeField]
    private List<Image> items;
    [SerializeField]
    private List<Image> powerups;
    [SerializeField]
    private Sprite noImage;

    public void drawItems (PlayerData data)
    {
        for(int i = 0; i < data.inventory.Count; ++i)
        {
            items[i].sprite = data.inventory[i].image;
        }
        for (int i = data.inventory.Count; i < items.Count; ++i)
        {
            items[i].sprite = noImage;
        }

        for (int i = 0; i < data.powerups.Count; ++i)
        {
            powerups[i].sprite = data.powerups[i].image;
        }
        for (int i = data.powerups.Count; i < powerups.Count; ++i)
        {
            powerups[i].sprite = noImage;
        }
    }

    public Transform getTransformOfItem(int id)
    {
        return items[id].gameObject.transform;
    }

    public Transform getTransformOfPowerup(int id)
    {
        return powerups[id].gameObject.transform;
    }
}