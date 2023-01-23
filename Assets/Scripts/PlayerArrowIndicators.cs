using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowIndicators : MonoBehaviour
{
    private PlayerController player;
    public LayerMask downArrowRequirements;
    public GameObject downArrowObject;
    public LayerMask upArrowRequirements;
    public GameObject upArrowObject;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        downArrowObject.SetActive((player.isTouching(downArrowRequirements) || player.isStandingOnGlass && player.playerData.cutscene.Contains("powerup.boots")) && !player.pauseInput);
        upArrowObject.SetActive(player.isTouching(upArrowRequirements) && !player.onLadder && player.playerData.cutscene.Contains("powerup.gloves"));
    }
}
