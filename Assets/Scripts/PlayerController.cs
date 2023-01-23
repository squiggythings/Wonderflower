using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 speed;
    [Header("Walking Variables")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float walkFriction;

    [Header("Jump Variables")]
    [SerializeField]
    private int maxJumpFrames = 12;
    [SerializeField]
    private float jumpStrength;

    [Header("Gravity Variables")]
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float maxFallVelocity;
    [SerializeField]
    private float windStrength;
    [SerializeField]
    private float windMaximumUpSpeed;

    [Header("Climbing Variables")]
    [SerializeField]
    private float climbUpSpeed;
    [SerializeField]
    private float climbDownSpeed;

    [Space]
    public InputManager input;
    public PlayerAudio sounds;
    public PlayerData playerData;
    public PlayerAnimator animator;
    public CamFollow cam;
    public ParticleManager particles;
    public BoxCollider2D boxCol;
    public CutsceneTrigger chainCutsceneEvent;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlatform;
    public LayerMask whatIsLadder;
    public LayerMask whatIsWind;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsNPC;
    public LayerMask whatIsGlass;
    public LayerMask concreteTerrainLayer;
    public Terrain terrain;
    [SerializeField]
    private int touch;
    [SerializeField]
    private int canJump;
    public bool onGround;
    public int direction = 1;
    public bool onLadder;
    public int falling, land, climbing;
    public int climbAnim;
    private bool isGrabSlideOnToChain;
    private int chainTimer;
    public int wasOffGround;
    public bool isStandingOnGlass;
    public bool pauseInput = false;
    public bool pausePhysics = false;
    bool dirBeforeClimb;
    private Vector2 translateOverflow;
    int chainWarningTimer; // prevents the chain warning cutscene from replaying immediately
    private int moveDuringCutscenes;
    private bool switchDirWhenFinishedCutsceneMove;
    Vector2 lastSafePosition;

    // Start is called before the first frame update
    void Awake()
    {
        sounds = gameObject.GetComponent<PlayerAudio>();
        input = FindObjectOfType<InputManager>();
        cam = FindObjectOfType<CamFollow>();
        particles = FindObjectOfType<ParticleManager>();
        //playerData = new PlayerData();
        //SAveData = FindObjectOfType<SaveData>();
    }

    void Start()
    {

    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (playerData != null)
        {
            doPlayerTick();
            updatePlayerData(playerData);
            cam.Setup();
        }
        else
        {
            playerData = new PlayerData();
            playerData.loadFromPlayerPrefs();
            goToPositionOf(playerData);
            
        }
    }

    public void save()
    {
        playerData.positionX = (int)lastSafePosition.x;
        playerData.positionY = (int)lastSafePosition.y;
        playerData.saveToPlayerPrefs();
    }

    void setLastSafePosition()
    {
        if (land > 60 && Mathf.Abs(cam.playerScreenPosition().x) <= 98)
        {
            lastSafePosition.x = transform.position.x;
            lastSafePosition.y = transform.position.y;
        }
    }

    void doPlayerTick()
    {

        if (!pausePhysics)
        {
            playerControlsLeftRight();
            playerGravity();
            playerControlsJump();
            moveX(speed.x * 0.0166666667f);
            if (Mathf.Abs(speed.x) > 10) direction = (int)Mathf.Sign(speed.x);

        }
        animator.tick();
        if (moveDuringCutscenes != 0)
            moveDuringCutscenes -= (int)Mathf.Sign(moveDuringCutscenes);
        else if (switchDirWhenFinishedCutsceneMove && Mathf.Abs(speed.x) < 10)
        {
            land = -2;
            switchDirWhenFinishedCutsceneMove = false;
            direction = -direction;
        }
        setTerrain();
    }

    void playerControlsLeftRight()
    {
        if (onLadder)
        {
            speed.x = 0;
        }
        else
        {
            speed.x *= walkFriction;
            if (input.GetButton(Actions.Left) && !pauseInput) speed.x -= walkSpeed;
            if (input.GetButton(Actions.Right) && !pauseInput) speed.x += walkSpeed;
            if (moveDuringCutscenes != 0)
                speed.x += Mathf.Sign(moveDuringCutscenes) * walkSpeed * 0.8f;
        }
    }

    void setTerrain()
    {

        if (cam.environment == Environment.SEWERS)
            terrain = Terrain.wet;
        else if (cam.environment == Environment.HIGHLOFT)
        {
            terrain = Terrain.carpet;
            if (isTouching(concreteTerrainLayer))
                terrain = Terrain.concrete;
        }
        else
        {
            terrain = Terrain.concrete;
        }
        if (isStandingOnGlass) terrain = Terrain.glass;
    }

    public void StartCutsceneMoveBack()
    {
        moveDuringCutscenes = -direction * 16;
        switchDirWhenFinishedCutsceneMove = true;
    }

    public void StartCutsceneMoveOnlyALittle()
    {
        moveDuringCutscenes = -direction * 9;
        switchDirWhenFinishedCutsceneMove = true;
    }

    public void MoveInCutscene(int amt)
    {
        moveDuringCutscenes = direction * amt;
        switchDirWhenFinishedCutsceneMove = false;
    }

    void getOntoLadder()
    {
        dirBeforeClimb = animator.sprite.flipX;
        onLadder = true;
        speed.x = 0;
        transform.position = new Vector3(Mathf.Round((transform.position.x + 8) / 16) * 16 - 8, transform.position.y, transform.position.z);
        sounds.playSoundGrabChain();
        climbing = 0;
        chainTimer = 0;
        if (speed.y < -100)
        {
            sounds.playSoundChainSlide(Mathf.Abs(speed.y));
            isGrabSlideOnToChain = true;
        }
        else
        {
            isGrabSlideOnToChain = false;
        }
    }

    public void onBreakGlass()
    {
        speed.y = 160;
    }

    void doLadderMovements()
    {
        ++chainTimer;
        if (speed.y < -20)
        {
            speed.y *= 0.666f;
            if (Time.frameCount % 2 == 0)
                particles.createParticle(particles.slide, transform.position + new Vector3(0, 8, 0), 2, 8, 1);
        }
        else
        {
            isGrabSlideOnToChain = false;
            speed.y = 0;
        }
        if (input.GetButton(Actions.Up) && !pauseInput && !isGrabSlideOnToChain)
        {
            if (Mathf.Abs(climbing) % animator.climbFrameLength == 0)
            {
                climbAnim = 1 - climbAnim;
                sounds.playSoundClimbChain();
            }
            ++climbing;
            speed.y = climbUpSpeed;
        }
        if (input.GetButton(Actions.Down) && !pauseInput)
        {
            isGrabSlideOnToChain = true;
            speed.y = -climbDownSpeed;
            if (input.GetButtonDown(Actions.Down))
            {
                sounds.playSoundChainSlide(Mathf.Abs(speed.y));
            }
        }
        if (!isTouching(whatIsLadder))
        {
            onLadder = false;
            animator.sprite.flipX = dirBeforeClimb;
            if (speed.y > 0)
            {
                speed.y = climbUpSpeed * 2.5f;
                moveY(2);
            }
            else
            {
                speed.y = 0;
            }
        }
        if (onGround && chainTimer > 5)
        {
            onLadder = false;
            animator.sprite.flipX = dirBeforeClimb;
        }
    }

    void playerGravity()
    {
        if (onLadder)
        {
            doLadderMovements();
        }
        else
        {
            speed.y -= gravity;
            if (isTouching(whatIsWind) && playerData.cutscene.Contains("powerup.feather"))
            {
                speed.y += windStrength;
                if (speed.y > windMaximumUpSpeed) speed.y = windMaximumUpSpeed;
            }
            if (speed.y < -maxFallVelocity) speed.y = -maxFallVelocity;
        }
        moveY(speed.y * 0.0166666667f);
        onGround = isOnGround(transform.position);
        if (onGround)
        {
            if (wasOffGround > 0)
            {
                // EVENT_LAND
                if (isGlassBelow())
                    terrain = Terrain.glass;
                sounds.playSoundLand(terrain, Mathf.Abs(speed.y));
                if (speed.y < -100 && terrain != Terrain.carpet)
                    particles.createParticle(particles.land, transform.position, 1);
                wasOffGround = 0;
            }
            speed.y = 0;
            if (!input.GetButton(Actions.Jump))
                canJump = maxJumpFrames;
            ++land;
            falling = 0;
        }
        else if (onLadder)
        {
            if (!input.GetButton(Actions.Jump))
                canJump = maxJumpFrames;
        }
        else
        {
            if (!input.GetButton(Actions.Jump) && falling > 2)
                canJump = 0;
            wasOffGround = 3;
            land = -2;
            ++falling;
        }
    }

    void playerControlsJump()
    {
        if (canJump > 0)
        {
            if (input.GetButton(Actions.Jump) && !pauseInput)
            {
                onGround = false;
                if (input.GetButtonDown(Actions.Jump))
                {
                    AudioManager.instance.playSoundEffect(sounds.jump);
                    if (!onLadder && falling < 2)
                    {
                        if (terrain != Terrain.carpet)
                        { particles.createParticle(particles.jump, transform.position, 1); }
                    }
                    else
                        particles.createParticle(particles.slide, transform.position + new Vector3(0, 4, 0), 6, 2, 3);
                    // EVENT_JUMP
                }
                speed.y = jumpStrength;
                onLadder = false;
                canJump--;
            }
        }
        if (input.GetButtonUp(Actions.Jump))
        {
            canJump = 0;
        }
        if (input.GetButton(Actions.Up) && !onLadder)
        {
            if (input.GetButton(Actions.Jump) && canJump < 1 || !input.GetButton(Actions.Jump))
                if (isTouching(whatIsLadder))
                {
                    if (playerData.cutscene.Contains("powerup.gloves"))
                        getOntoLadder();
                    else
                    {
                        if (chainWarningTimer == 0)
                        {
                            chainWarningTimer = 1;
                            sounds.playSoundChainSlide(60);
                            chainCutsceneEvent.triggerCutscene();
                        }
                    }
                }
        }
        if (!input.GetButton(Actions.Up))
            chainWarningTimer = 0;
    }

    public void moveY(float amount)
    {
        translateOverflow.y += amount;
        int move = (int)Mathf.Round(translateOverflow.y - 0.5f);
        if (move != 0)
        {
            translateOverflow.y -= move;
            int sign = (int)Mathf.Sign(move);
            while (move != 0)
            {
                if (!collideAt(sign > 0, transform.position + new Vector3(0, sign, 0)))
                {
                    //There is no Solid immediately beside us 
                    transform.Translate(0, sign, 0);
                    move -= sign;
                }
                else
                {
                    //Hit a solid!
                    collideY(sign);
                    return;
                }
            }
        }
    }

    void collideY(int sign)
    {
        if (sign > 0)
        {
            if (!onGround && speed.y > 50f)
            {
                // EVENT_HIT_HEAD
                particles.createParticle(particles.hitHead, transform.position, 1);
                sounds.playSoundLand(Terrain.concrete, Mathf.Abs(speed.y));
            }
            speed.y = -17f;
            if (isTouching(whatIsWind) && playerData.cutscene.Contains("powerup.feather"))
            {
                speed.y -= windStrength * 3;
            }
            canJump = 0;
        }
    }

    public void moveX(float amount)
    {
        translateOverflow.x += amount;
        int move = (int)Mathf.Round(translateOverflow.x);
        if (move != 0)
        {
            translateOverflow.x -= move;
            int sign = (int)Mathf.Sign(move);
            while (move != 0)
            {
                if (!collideAt(sign > 0, transform.position + new Vector3(sign, 0, 0)))
                {
                    //There is no Solid immediately beside us 
                    transform.Translate(sign, 0, 0);
                    move -= sign;
                }
                else
                {
                    //Hit a solid!
                    break;
                }
            }
        }
    }

    bool collideAt(bool up, Vector3 position)
    {
        if (!up && checkIfInsidePlatform(position))
        {
            return Physics2D.OverlapBox(new Vector2(position.x, position.y) + boxCol.offset, boxCol.size, 0, whatIsPlatform);

        }
        else
        {
            return Physics2D.OverlapBox(new Vector2(position.x, position.y) + boxCol.offset, boxCol.size, 0, whatIsGround);
        }
    }
    bool isOnGround(Vector3 position)
    {
        if (speed.y < 0 && checkIfInsidePlatform(position))
            return Physics2D.OverlapBox(new Vector2(position.x, position.y - 1) + boxCol.offset, boxCol.size, 0, whatIsPlatform);
        else
            return Physics2D.OverlapBox(new Vector2(position.x, position.y - 1) + boxCol.offset, boxCol.size, 0, whatIsGround);
    }

    void updatePlayerData(PlayerData data)
    {
        data.positionX = (int)transform.position.x;
        data.positionY = (int)transform.position.y;
    }

    void goToPositionOf(PlayerData data)
    {
        //playerData = FindObjectOfType<SaveData>().saveData;
        transform.position = new Vector3(data.positionX, data.positionY, 0);
    }

    private Vector2 getPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    private bool checkIfInsidePlatform(Vector3 position)
    {
        return Physics2D.OverlapBox(new Vector2(position.x, position.y), new Vector2(boxCol.size.x, 1), 0, whatIsPlatform);
    }
    public bool isTouching(LayerMask layer) => Physics2D.OverlapBox(getPosition() + boxCol.offset, boxCol.size, 0, layer);

    public bool isGlassBelow() => Physics2D.OverlapBox(getPosition() + boxCol.offset - new Vector2(0, 4), new Vector2(2, boxCol.size.y), 0, whatIsGlass);
}
