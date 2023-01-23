using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    PlayerController player;
    public string unlockWhenCutsceneHas;
    bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.playerData != null)
        {
            if (player.playerData.cutscene.Contains(unlockWhenCutsceneHas) && locked)
            {
                locked = false;
                transform.Translate(0, 24, 0);
                player.particles.createParticle(player.particles.slide, transform.position, 8, 7, 8);
            }

        }
    }
}