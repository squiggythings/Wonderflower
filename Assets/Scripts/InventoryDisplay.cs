using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class InventoryDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject container;
    public InventoryItemRenderer itemRenderer;
    private PlayerController player;
    [SerializeField]
    private Transform scrollingBackground;
    [SerializeField]
    private TextMeshProUGUI itemTitle;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    private CamFollow cam;
    public Tilemap mapTiles;
    public Transform cursor;
    public Transform mapMarker;
    [SerializeField]
    private Tile regularTile;
    [SerializeField]
    private Tile bottomRowTile;
    [Header("Data")]
    public int cursorIndex;

    [Header("Tweaks")]
    public float BGScrollSpeed;

    [Header("Sounds")]
    public SoundEffect inventoryOpen;
    public SoundEffect inventoryClose;
    public SoundEffect inventoryBlip;
    public static bool isInventoryOpen;

    private InventoryObject currentObject;
    void Start()
    {
        cam = FindObjectOfType<CamFollow>();
        player = FindObjectOfType<PlayerController>();
    }

    public void activateInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (isInventoryOpen)
        {
            player.pauseInput = true;
            player.pausePhysics = true;
            AudioManager.instance.playSoundEffect(inventoryOpen);
        }
        else
        {
            player.pauseInput = false;
            player.pausePhysics = false;
            AudioManager.instance.playSoundEffect(inventoryClose);
        }
    }

    // Update is called once per frame
    void Update()
    {
        container.SetActive(isInventoryOpen);
        if (isInventoryOpen)
        {
            scrollingBackground.transform.localPosition = new Vector3(Time.time * BGScrollSpeed % 32, -Time.time * BGScrollSpeed % 32, 0);
            mapTiles.ClearAllTiles();
            foreach (Vector3Int scr in player.playerData.unvisitedScreens)
            {
                if (scr.y > -5)
                    mapTiles.SetTile(scr, regularTile);
                else
                    mapTiles.SetTile(scr, bottomRowTile);
            }

            Vector3Int screen = cam.getScreenCoordinates();
            mapMarker.localPosition = new Vector3(screen.x * 14 + 8, screen.y * 11 + 6, 0);

            itemRenderer.drawItems(player.playerData);

            if (player.input.GetButtonDown(Actions.Up))
                moveCursor(0, -1);
            if (player.input.GetButtonDown(Actions.Down))
                moveCursor(0, 1);
            if (cursorIndex % 2 == 0)
            {
                if (player.input.GetButtonDown(Actions.Right))
                    moveCursor(1, 0);
            }
            else
            {
                if (player.input.GetButtonDown(Actions.Left))
                    moveCursor(-1, 0);
            }
            if (cursorIndex < 10)
            {
                cursor.parent = itemRenderer.getTransformOfItem(cursorIndex);
                if (cursorIndex < player.playerData.inventory.Count)
                {
                    currentObject = player.playerData.inventory[cursorIndex];
                }
                else
                {
                    currentObject = null;
                }
            }
            else
            {
                cursor.parent = itemRenderer.getTransformOfPowerup((cursorIndex - 10) / 2);
                if ((cursorIndex - 10) / 2 < player.playerData.powerups.Count)
                {
                    currentObject = player.playerData.powerups[(cursorIndex - 10) / 2];
                }
                else
                {
                    currentObject = null;
                }
            }

            if (currentObject == null)
            {
                itemTitle.text = " ";
                itemDescription.text = " ";
            }
            else
            {
                bool ispowerup = currentObject.type == InventoryObject.ObjectType.Powerup;
                itemTitle.text = (ispowerup ? "   * " : "") + currentObject.displayName + (ispowerup ? " *" : "");
                itemDescription.text = currentObject.description;
            }
            cursor.localPosition = Vector3.zero;
        }
    }

    void moveCursor(int x, int y)
    {
        AudioManager.instance.playSoundEffect(inventoryBlip);
        if (cursorIndex >= 10)
            x = 0;
        cursorIndex += x + 2 * y;
        if (cursorIndex < 0 || cursorIndex > 15) cursorIndex -= x + 2 * y;
    }


}
