using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentNameCard : MonoBehaviour
{
    SpriteRenderer sprite;
    CamFollow cam;
    public List<Sprite> sprites;
    public int fadeTime;
    public int waitTime;
    public Environment currentEnv;
    public List<Vector3Int> blackGroundlines;
    Coroutine anim;
    Dictionary<Environment, Sprite> list = new Dictionary<Environment, Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cam = FindObjectOfType<CamFollow>();
        list.Add(Environment.HIGHLOFT, sprites[0]);
        list.Add(Environment.GROUNDLINE, sprites[1]);
        list.Add(Environment.SEWERS, sprites[3]);
        list.Add(Environment._BEGINNING, null);
        list.Add(Environment._ENDING, null);
        list.Add(Environment.NULL, null);
    }

    public void newEnvironment(Environment e)
    {
        currentEnv = e;
        if (anim != null)
            StopCoroutine(anim);
        anim = StartCoroutine(animate());
    }

    IEnumerator animate()
    {
        yield return new WaitForSeconds(0.2f);
        sprite.sprite = list[currentEnv];
        if (blackGroundlines.Contains(cam.getScreenCoordinates()))
            sprite.sprite = sprites[2];
        sprite.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < fadeTime; ++i)
        {
            sprite.color = new Color(1, 1, 1, i / (float)fadeTime);
            yield return null;
        }
        sprite.color = new Color(1, 1, 1, 1);
        for (int i = waitTime; i > 0; --i)
        {
            yield return null;
        }
        for (int i = fadeTime; i > 0; --i)
        {
            sprite.color = new Color(1, 1, 1, i / (float)fadeTime);
            yield return null;
        }
        sprite.color = new Color(1, 1, 1, 0);
    }
}
