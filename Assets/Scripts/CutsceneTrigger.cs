using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
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
        Debug.LogError("No Valid Cutscene for trigger: " + name);
        return null;
    }

    private void Start()
    {
        csc = FindObjectOfType<CutscenePlayer>();
        player = FindObjectOfType<PlayerController>();
    }

    public void triggerCutscene()
    {
        csc.StartCutscene(GetFirstFittingCutscene());
    }
}
