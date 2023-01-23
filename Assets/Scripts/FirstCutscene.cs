using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCutscene : MonoBehaviour
{
    public PlayerController player;
    public NPCDialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    private void Update()
    {

    }

    public IEnumerator wait()
    {
        while (player.playerData == null)
            yield return null;
        if (!player.playerData.cutscene.Contains("firstCSC"))
        {
            dialogue.activate();
        }
    }
}
