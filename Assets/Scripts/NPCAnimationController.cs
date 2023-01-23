using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteAnimator animator;
    public PlayerController player;

    public List<AnimationFrame> defaultAnimation;
    public bool flipToFacePlayer;
    [Header("Flip Dependent")]
    public bool useDifferentAnimationForLeft;
    public List<AnimationFrame> animationLeft;

    [Header("Talking Animation")]
    public bool useTalkingAnimation;
    public List<AnimationFrame> talkingAnimation;

    [Header("Progression Dependent Animations")]
    [Tooltip("Leave blank for no progression dependent animation")]
    public string cutsceneMustContain;
    public List<AnimationFrame> secondaryAnimation;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (player.playerData != null)
        {
            if (useTalkingAnimation)
            {
                if (CutscenePlayer.isNPCTalking)
                    animator.playAnimation(talkingAnimation);
                else
                    animator.playAnimation(defaultAnimation);
            }
            else if (flipToFacePlayer)
            {
                spriteRenderer.flipX = player.transform.position.x < transform.position.x;
                if (useDifferentAnimationForLeft)
                {
                    if (spriteRenderer.flipX)
                        animator.playAnimation(animationLeft);
                    else
                        animator.playAnimation(defaultAnimation);
                }
                else
                {
                    animator.playAnimation(defaultAnimation);
                }
                if (cutsceneMustContain != "")
                {
                    if (player.playerData.cutscene.Contains(cutsceneMustContain))
                    {
                        animator.playAnimation(secondaryAnimation);
                    }
                }
            }
            else if (cutsceneMustContain != "")
            {
                if (player.playerData.cutscene.Contains(cutsceneMustContain))
                {
                    animator.playAnimation(secondaryAnimation);
                }
            }
            else
            {
                animator.playAnimation(defaultAnimation);
            }
        }
    }
}
