using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGlass : MonoBehaviour
{
    public PlayerController player;
    public ParticleManager particles;
    public SoundEffect sound;
    public BoxCollider2D boxCol;
    public LayerMask whatIsPlayer;

    [Tooltip("Is stored in PlayerData.cutscene when broken")]
    public string uniqueID;
    // Start is called before the first frame update
    void Start()
    {
        particles = FindObjectOfType<ParticleManager>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.playerData != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 100)
            {
                player.isStandingOnGlass = isPlayerIsStandingOn();
                if (player.isStandingOnGlass && player.input.GetButtonDown(Actions.Down) && player.playerData.cutscene.Contains("powerup.boots"))
                {
                    shatter();
                }
            }
            if (player.playerData.cutscene.Contains(uniqueID)) Destroy(gameObject);
        }
    }

    void shatter()
    {
        player.isStandingOnGlass = false;
        player.onBreakGlass();
        particles.createParticle(particles.glass, transform.position, 16, 4, 25);
        AudioManager.instance.playSoundEffect(sound);
        player.playerData.cutscene.Add(uniqueID);
        Destroy(gameObject);
    }

    bool isPlayerIsStandingOn()
    {
        return Physics2D.OverlapBox(getPosition() + boxCol.offset + Vector2.up, boxCol.size, 0, whatIsPlayer);
    }

    Vector2 getPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
