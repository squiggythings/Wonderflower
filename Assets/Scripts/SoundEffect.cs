using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Sound Effect", order = 2)]
public class SoundEffect : ScriptableObject
{
    [SerializeField]
    [Tooltip("The variations of the sound")]
    private AudioClip[] clips;
    [SerializeField]
    [Tooltip("Repitch if needed")]
    private float pitchOffset = 0.0f;
    [Range(0, 1)]
    [SerializeField]
    [Tooltip("How much can the pitch be altered?")]
    private float pitchVariation = 0.1f;
    [Range(0.01f, 7f)]
    [SerializeField]
    [Tooltip("How loud is this sound effect?")]
    private float mixingMultiplier = 1f;

    public AudioClip GetClip()
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("Sound Effect \"" + name + "\" needs at least one AudioClip!");
            return null;
        }
        else return clips[Random.Range(0, clips.Length)];
    }

    public float GetPitch() => 1 + (Random.Range(-pitchVariation, pitchVariation) + pitchOffset)/ 5f;

    public float GetVolumeMultiplier() => mixingMultiplier;
}
