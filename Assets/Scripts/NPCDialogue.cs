using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCDialogue : MonoBehaviour
{
    private BoxCollider2D talkZone;
    [Tooltip("Name of the NPC")]
    public string NPCName;
    [Tooltip("NPC will go through cutscene events from top to bottom." +
        "\nPlace the earlier events at the bottom of the list.")]
    public List<CutsceneEvent> dialogueEvents;
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
        Debug.LogError("No Valid Cutscene for character: " + name);
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
        if (player.input.GetButtonDown(Actions.Down) && !player.pauseInput){
            if (isTouching(player.whatIsPlayer))
            {
                activate();
            }
        }
    }

    public void activate()
    {
        csc.StartCutscene(GetFirstFittingCutscene(), this);
    }

    Vector2 getPosition() => new Vector2(transform.position.x, transform.position.y);

    bool isTouching(LayerMask layer) => Physics2D.OverlapBox(getPosition() + talkZone.offset, talkZone.size, 0, layer);
}
