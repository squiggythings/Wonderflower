using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonTitle : MonoBehaviour
{
    public Sprite[] idle = new Sprite[2];
    private SpriteRenderer sprite;
    private int peckAnim = 0;
    private bool flip;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        flip = Random.Range(0.0f, 1.0f) < 0.5f;
        StartCoroutine(mainPeckLoop());

    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = flip;
        sprite.sprite = idle[peckAnim];
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
}
