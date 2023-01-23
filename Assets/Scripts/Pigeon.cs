using UnityEngine;
using System.Collections;

public class Pigeon : MonoBehaviour
{
    private Vector3 origin;
    private PlayerController player;
    private SpriteRenderer sprite;

    public Sprite[] idle = new Sprite[2];
    public Sprite[] flap = new Sprite[2];

    private bool hasFlown;
    public SoundEffect wingsFlutter;
    private int peckAnim = 0;
    private bool flip;
    public float radius;

    // Use this for initialization
    void Start()
    {
        origin = transform.position;
        origin.z = 0; // just in case it isnt
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        flip = Random.Range(0.0f, 1.0f) < 0.5f;
        StartCoroutine(mainPeckLoop());
        resetPigeon();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position + new Vector3(0,8,0)) < radius && !hasFlown)
        {
            hasFlown = true;
            StartCoroutine(fly());
        }
        if (hasFlown)
        {
            sprite.sprite = flap[(int)Mathf.Round(Time.frameCount / 5) % 2];
        }
        else
        {
            sprite.flipX = flip;
            sprite.sprite = idle[peckAnim];
        }
    }

    public IEnumerator mainPeckLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.9f));
            peckAnim = 1;
            yield return new WaitForSeconds(0.1f);
            peckAnim = 0;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.9f));
            peckAnim = 1;
            yield return new WaitForSeconds(0.1f);
            peckAnim = 0;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            if (Random.Range(0.0f, 1.0f) < 0.5f)
                flip = !flip;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.9f));
            peckAnim = 1;
            yield return new WaitForSeconds(0.1f);
            peckAnim = 0;
            yield return new WaitForSeconds(Random.Range(0.8f, 2.5f));
            flip = !flip;
        }
    }

    public IEnumerator fly()
    {
        float speed = 140;
        int dir = (player.transform.position.x < transform.position.x) ? 1 : -1;
        sprite.flipX = dir < 0;
        AudioManager.instance.playSoundEffect(wingsFlutter);
        for (int i = 0; i < 500; i++)
        {
            if (i < 20)
                speed += 0.25f;
            transform.Translate(speed * Time.deltaTime * dir, speed * Time.deltaTime * 0.8f, 0);
            yield return null;
        }

        // hide once moved
        sprite.enabled = false;
        transform.position = origin;

        // wait until the player is far enough away from the point of origin before re-spawning
        while (Vector3.Distance(transform.position, player.transform.position) < 450)
            yield return null;

        // then reset
        resetPigeon();
    }

    void resetPigeon()
    {
        transform.position = origin;
        hasFlown = false;
        sprite.enabled = true;
    }
}
