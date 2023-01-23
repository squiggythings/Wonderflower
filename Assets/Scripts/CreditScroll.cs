using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    public float speed;
    public AudioSource music;
    public static float POSITION, lastPosition;
    public Transform creditText;
    public Transform endImage;
    public SoundEffect musicVolume;
    public float musicMixer;
    void Start()
    {
        POSITION = creditText.transform.position.y;
        StartCoroutine(scroll());
    }

    // Update is called once per frame
    void Update()
    {
        music.volume = GameOptions._MUSVOL / 10f * musicVolume.GetVolumeMultiplier() * musicMixer;
    }

    IEnumerator scroll()
    {
        yield return new WaitForSeconds(1.5f);
        POSITION = creditText.transform.position.y;
        while (POSITION < 2078)
        {
            lastPosition = POSITION;
            POSITION += speed;

            creditText.position = new Vector3(0, POSITION, 0);
            yield return null;
        }
        POSITION = 2078;
        creditText.position = new Vector3(0, Mathf.Round(POSITION), 0);
    }
}
