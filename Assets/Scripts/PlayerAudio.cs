using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public SoundEffect jump;
    public SoundEffect land_concrete;
    public SoundEffect splash;
    public SoundEffect land_carpet;
    public SoundEffect land_glass;
    public SoundEffect footstep_concrete;
    public SoundEffect footstep_carpet;
    public SoundEffect footstep_glass;
    public SoundEffect footstep_wet;
    public SoundEffect chain_grab;
    public SoundEffect chain_slide;
    public SoundEffect chain_climb;

    public void playSoundLand(Terrain terrain, float speed)
    {
        float dropoff = 380f;
        float floor = 0.20f;
        switch (terrain)
        {
            case Terrain.concrete:
                AudioManager.instance.playSoundEffect(land_concrete, speed / dropoff + floor);
                break;
            case Terrain.wet:
                AudioManager.instance.playSoundEffect(land_concrete, speed / dropoff + floor);
                AudioManager.instance.playSoundEffect(splash, speed / dropoff + floor);
                break;
            case Terrain.carpet:
                AudioManager.instance.playSoundEffect(land_carpet, speed / dropoff + floor);
                break;
            case Terrain.glass:
                AudioManager.instance.playSoundEffect(land_glass, speed / dropoff + floor);
                break;
        }
    }

    public void playSoundJump(Terrain terrain)
    {
        switch (terrain)
        {
            case Terrain.wet:
                AudioManager.instance.playSoundEffect(jump);
                AudioManager.instance.playSoundEffect(splash);
                break;
            default:
                AudioManager.instance.playSoundEffect(jump);
                break;
        }
    }

    public void playSoundGrabChain()
    {
        AudioManager.instance.playSoundEffect(chain_grab);
    }

    public void playSoundClimbChain()
    {
        AudioManager.instance.playSoundEffect(chain_climb);
    }

    public void playSoundFootstep(Terrain terrain)
    {
        switch (terrain)
        {
            case Terrain.concrete:
                AudioManager.instance.playSoundEffect(footstep_concrete);
                break;
            case Terrain.wet:
                AudioManager.instance.playSoundEffect(footstep_wet);
                break;
            case Terrain.carpet:
                AudioManager.instance.playSoundEffect(footstep_carpet);
                break;
            case Terrain.glass:
                AudioManager.instance.playSoundEffect(footstep_glass);
                break;
        }
    }

    public void playSoundChainSlide(float speed)
    {
        float dropoff = 280f;
        float floor = 0.20f;
        AudioManager.instance.playSoundEffect(chain_slide, speed / dropoff + floor);
    }

}
public enum Terrain
{
    concrete,
    wet,
    carpet,
    glass,
}
