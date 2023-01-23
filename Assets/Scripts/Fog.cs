using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour
{
    CamFollow cam;
    float x;
    float alpha;
    public float speed, offset, amplitude, baseAlpha;
    public float alphaAmplitude;
    float a;
    SpriteRenderer sprite;
    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cam = FindObjectOfType<CamFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha += (((cam.environment == Environment.SEWERS || cam.showWonderFlower )? 1 : 0) - alpha) * 0.2f; 
        a = Mathf.Sin(Time.time * speed * 6.8f) * alphaAmplitude;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (a + baseAlpha) * alpha);
        x = Mathf.Sin(Time.time * speed + offset) * amplitude;
        transform.localPosition = new Vector3(Mathf.Round(x), 0, 10);
    }
}
