using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private BoxCollider2D talkZone;
    [Tooltip("Item will go through cutscene events from top to bottom." +
        "\nPlace the earlier events at the bottom of the list.")]
    public List<CutsceneEvent> dialogueEvents;
    public string destroyIfCutsceneContains;
    public PlayerController player;
    private CutscenePlayer csc;
    public CutsceneEvent GetFirstFittingCutscene()
    {
        foreach (CutsceneEvent c in dialogueEvents)
        {
            if (c.meetsRequirements(player.playerData))
            {
                return c;
            }
        }
        Debug.LogError("No Valid Cutscene for item: " + name);
        return null;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        csc = FindObjectOfType<CutscenePlayer>();
        talkZone = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (player.playerData != null)
        {
            if (player.playerData.cutscene.Contains(destroyIfCutsceneContains)) Destroy(gameObject);
            if (player.input.GetButtonDown(Actions.Down) && !player.pauseInput)
            {
                if (isTouching(player.whatIsPlayer))
                {
                    csc.StartCutscene(GetFirstFittingCutscene());
                }
            }
        }
    }

    Vector2 getPosition() => new Vector2(transform.position.x, transform.position.y);

    bool isTouching(LayerMask layer) => Physics2D.OverlapBox(getPosition() + talkZone.offset, talkZone.size, 0, layer);
}
