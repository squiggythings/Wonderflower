using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneContainsToggle : MonoBehaviour
{
    public string showIfCutsceneContains;
    SpriteRenderer sprite;
    PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.playerData != null)
            sprite.enabled = player.playerData.cutscene.Contains(showIfCutsceneContains);
    }
}
