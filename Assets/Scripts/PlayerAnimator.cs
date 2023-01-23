using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerController controller;
    public SpriteRenderer sprite;
    public List<Sprite> idle;
    public float idleFrameLength;
    public List<Sprite> fall;
    public float fallFrameLength;
    public List<Sprite> jump;
    public float jumpFrameLength;
    public List<Sprite> run;
    public float runFrameLength;
    public List<Sprite> climb;
    public float climbFrameLength;
    private int lastRunAnim;

    public void tick()
    {
        sprite.flipX = controller.direction < 0;
        if (controller.onGround)
        {
            sprite.sprite = idle[(int)(controller.land / idleFrameLength) % idle.Count];
            if (Mathf.Abs(controller.speed.x) > 10f)
            {
                int runAnim = (int)(controller.land / runFrameLength) % run.Count;
                if (lastRunAnim == 0 || lastRunAnim == 2)
                {
                    if (runAnim == 1 || runAnim == 3)
                    {
                        controller.sounds.playSoundFootstep(controller.terrain);
                        if (controller.terrain == Terrain.wet) controller.particles.createParticle(controller.particles.footstepSplash, transform.position, 1, 0, 1);
                    }
                }
                sprite.sprite = run[runAnim];
                lastRunAnim = runAnim;
            }
        }
        else
        {
            if (controller.speed.y > 0)
            {
                sprite.sprite = jump[(int)(controller.falling / jumpFrameLength) % jump.Count];
            }
            else
            {
                sprite.sprite = fall[(int)(controller.falling / fallFrameLength) % fall.Count];
            }
        }
        if (controller.onLadder)
        {
            sprite.flipX = false;
            sprite.sprite = climb[controller.climbAnim];
        }
    }
}
