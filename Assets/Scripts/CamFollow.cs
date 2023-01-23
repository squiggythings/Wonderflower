using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CamFollow : MonoBehaviour
{
    public PlayerController player;
    public Environment environment;
    public EnvironmentNameCard envCard;
    [Range(0, 18)]
    public float animationSpeed;
    public float animSpeed;
    const int SCREEN_WIDTH = 224;
    const int SCREEN_HEIGHT = 176;
    public static bool isMoving;
    public SpriteRenderer instructions;

    [SerializeField]
    private Vector3Int currentScreenPos;
    public List<Vector3Int> waitUntilFinishedScrollingBeforeChangingEnvironment;
    public List<Vector3Int> sewerScreens;
    public List<Vector3Int> highloftScreens;
    public List<Vector3Int> groundlineScreens;
    public ParallaxBackground backgrounds;
    public bool showWonderFlower;
    bool start;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {

        if (!start && player.playerData != null)
        {
            if (MusicManager.instance != null)
            {
                if (player.playerData.cutscene.Contains("firstCSC"))
                    MusicManager.instance.switchEnvironment(15, environment, 0);
                snapToNearestScreenPosition(player.transform.position + Vector3.up * 8);
                player.playerData.visitScreen(getScreenCoordinates());
                start = true;
            }
        }
        if (player.playerData != null && start)
        {
            if (!isMoving)
            {
                if (Mathf.Abs(playerScreenPosition().x) > SCREEN_WIDTH / 2)
                    StartCoroutine(moveHorizontal(Mathf.Sign(playerScreenPosition().x)));
                if (Mathf.Abs(playerScreenPosition().y) > SCREEN_HEIGHT / 2)
                    StartCoroutine(moveVertical(Mathf.Sign(playerScreenPosition().y)));
            }
            currentScreenPos = getScreenCoordinates();
            backgrounds.doUpdate();
            if (player.playerData.cutscene.Contains("firstCSC"))
            {
                if (MusicManager.instance.currentSong != null)
                {
                    if (environment == Environment.HIGHLOFT || environment == Environment.GROUNDLINE || environment == Environment.SEWERS)
                        if (MusicManager.instance.currentSong.environment != environment)
                            MusicManager.instance.switchEnvironment(15, environment, 50);
                    if (envCard.currentEnv != environment && getScreenCoordinates() != new Vector3Int(4, -4, 0))
                        envCard.newEnvironment(environment);
                }
            }
        }
    }

    public void ShakePosition(int x, int y)
    {
        snapToNearestScreenPosition(player.transform.position + Vector3.up * 8);
        transform.Translate(x, y, 0);
    }
    public void Setup()
    {
        if (!start)
        {
            snapToNearestScreenPosition(player.transform.position + Vector3.up * 8);
            if (player.playerData.cutscene.Contains("firstCSC"))
                MusicManager.instance.switchEnvironment(15, environment, 0);
            player.playerData.visitScreen(getScreenCoordinates());
            start = true;
        }
    }

    public void setEnvironment(string s)
    {
        //Debug.Log(s);
        switch (s.ToUpper())
        {
            case "HIGHLOFT":
                environment = Environment.HIGHLOFT;
                break;
            case "GROUNDLINE":
                environment = Environment.GROUNDLINE;
                break;
            case "SEWERS":
                environment = Environment.SEWERS;
                break;
            case "_BEGINNING":
                environment = Environment._BEGINNING;
                break;
            case "_ENDING":
                environment = Environment._ENDING;
                break;
        }
    }

    public Vector2 playerScreenPosition()
        => new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y + 8 - transform.position.y);

    void snapToNearestScreenPosition(Vector3 pos)
    {
        transform.position = new Vector3(Mathf.Round(pos.x / 224f) * 224, Mathf.Round(pos.y / 176f) * 176, -10);
        getEnvironmentOnFinishScroll();
    }

    void getNextEnvironment(int x, int y)
    {
        if (environment != Environment._ENDING)
        {
            if (sewerScreens.Contains(getScreenCoordinatesN(x, y)) && !waitUntilFinishedScrollingBeforeChangingEnvironment.Contains(getScreenCoordinatesN(x, y)))
                environment = Environment.SEWERS;
            if (groundlineScreens.Contains(getScreenCoordinatesN(x, y)))
                environment = Environment.GROUNDLINE;
            if (highloftScreens.Contains(getScreenCoordinatesN(x, y)))
                environment = Environment.HIGHLOFT;
        }
    }

    void getEnvironmentOnFinishScroll()
    {
        if (environment != Environment._ENDING)
        {
            if (sewerScreens.Contains(getScreenCoordinatesN(0, 0)))
                environment = Environment.SEWERS;
            if (groundlineScreens.Contains(getScreenCoordinatesN(0, 0)))
                environment = Environment.GROUNDLINE;
            if (highloftScreens.Contains(getScreenCoordinatesN(0, 0)))
                environment = Environment.HIGHLOFT;
        }
    }

    public Vector3Int getScreenCoordinates()
    {
        return new Vector3Int((int)transform.position.x / SCREEN_WIDTH, (int)transform.position.y / SCREEN_HEIGHT, 0);
    }

    public Vector3Int getScreenCoordinatesN(int x, int y)
    {
        return new Vector3Int((int)transform.position.x / SCREEN_WIDTH + x, (int)transform.position.y / -SCREEN_HEIGHT + y, 0);
    }

    public IEnumerator moveHorizontal(float direction)
    {
        animSpeed = animationSpeed;
        if (GameOptions._INSTANTSCROLL) animSpeed = 0;
        instructions.enabled = false;
        getNextEnvironment((int)direction, 0);
        isMoving = true;
        player.pausePhysics = true;
        for (float i = 0; i < animSpeed; ++i)
        {
            player.moveX(12f / animSpeed * direction);
            transform.Translate(new Vector3(SCREEN_WIDTH / animSpeed * direction, 0, 0));
            backgrounds.doUpdate();
            yield return null;
        }
        player.pausePhysics = false;
        snapToNearestScreenPosition(player.transform.position + Vector3.up * 8);
        backgrounds.doUpdate();
        player.playerData.visitScreen(getScreenCoordinates());
        isMoving = false;
        yield return null;
    }
    public IEnumerator moveVertical(float direction)
    {
        animSpeed = animationSpeed;
        if (GameOptions._INSTANTSCROLL) animSpeed = 0;
        instructions.enabled = false;
        getNextEnvironment(0, -(int)direction);
        isMoving = true;
        player.pausePhysics = true;
        for (float i = 0; i < animSpeed; ++i)
        {
            player.moveY(8f / animSpeed * direction);
            transform.Translate(new Vector3(0, SCREEN_HEIGHT / animSpeed * direction, 0));
            backgrounds.doUpdate();
            yield return null;
        }
        player.pausePhysics = false;
        snapToNearestScreenPosition(player.transform.position + Vector3.up * 8);
        backgrounds.doUpdate();
        player.playerData.visitScreen(getScreenCoordinates());
        isMoving = false;
        yield return null;
    }
}

public enum Environment
{
    HIGHLOFT,
    GROUNDLINE,
    SEWERS,
    _BEGINNING,
    _ENDING,
    NULL,
}