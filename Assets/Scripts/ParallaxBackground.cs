using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public CamFollow cam;
    public GameObject bg1;
    public GameObject bg2;
    public GameObject bg3;
    public GameObject bgSewer;
    public GameObject clouds;


    public float globalOffset;
    public float highloftOffset;
    public Vector3 offsetY;
    public Vector3 offsetX;
    public Vector3 parallaxAmt;
    public bool isTitle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void doUpdate()
    {
        float y = transform.position.y + globalOffset;
        bgSewer.SetActive(cam.environment == Environment.SEWERS);
        bgSewer.transform.localPosition = roundVector3(transform.position.x * -0.5f % 32, transform.position.y * -0.5f % 32, 10);
        bg1.transform.localPosition = roundVector3(((-16000 + transform.position.x) * parallaxAmt.x + offsetX.x) % 160, y * -0.03f - offsetY.x, 10);
        bg2.transform.localPosition = roundVector3(((-16000 + transform.position.x) * parallaxAmt.y + offsetX.y) % 160, y * -0.055f - offsetY.y, 10);
        bg3.transform.localPosition = roundVector3(((-16000 + transform.position.x) * parallaxAmt.z + offsetX.z) % 160, y * -0.09f - offsetY.z, 10);
        clouds.transform.localPosition = roundVector3((-16000 + Time.time * 16 + transform.position.x) * -0.15f % 160, clouds.transform.localPosition.y, 10);
    }

    private void Update()
    {
        if(isTitle)
            clouds.transform.localPosition = roundVector3((-16000 + Time.time * 16 + transform.position.x) * -0.15f % 160, clouds.transform.localPosition.y, 10);
    }

    Vector3 roundVector3(float x, float y, float z)
    {
        return new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
    }
}
