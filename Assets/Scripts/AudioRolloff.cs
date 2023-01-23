using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRolloff : MonoBehaviour
{
    private AudioSource source;
    public float rolloffRadius;
    public Vector2 boundsSize;
    private Vector3 basePos;
    [Range(0, 3f)]
    public float mixingMultiplier = 1;
    private PlayerController player;
    public void Start()
    {
        source = GetComponent<AudioSource>();
        basePos = transform.position;
        player = FindObjectOfType<PlayerController>();
    }

    public void Update()
    {
        float x = Mathf.Clamp(player.transform.position.x, basePos.x, getMaximum().x);
        float y = Mathf.Clamp(player.transform.position.y, basePos.y, getMaximum().y);
        transform.position = new Vector3(x, y, 0);

        float distVolume = 1 - Vector3.Distance(transform.position, player.transform.position) / rolloffRadius;
        source.volume = distVolume * mixingMultiplier * AudioManager.instance.getSFXVolume() * (PauseScreenDisplay.isPaused ? 0 : 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rolloffRadius);
        Gizmos.DrawWireCube(getAverage(), new Vector3(boundsSize.x, boundsSize.y, 0));
    }

    private Vector3 getMaximum()
    {
        return basePos + new Vector3(boundsSize.x, boundsSize.y, 0);
    }

    private Vector3 getAverage()
    {
        return new Vector3(transform.position.x + boundsSize.x / 2f, transform.position.y + boundsSize.y / 2f, 0);
    }
}
