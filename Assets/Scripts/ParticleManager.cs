using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject jump;
    public GameObject land;
    public GameObject slide;
    public GameObject glass;
    public GameObject footstepSplash;
    public GameObject hitHead;



    public void createParticle(GameObject particle, Vector3 position, int quantity)
    {
        for (int i = 0; i < quantity; ++i)
            Instantiate(particle, position, transform.rotation, transform);
    }

    public void createParticle(GameObject particle, Vector3 position, int randomX, int randomY, int quantity)
    {
        for (int i = 0; i < quantity; ++i)
            Instantiate(particle, position + new Vector3(Random.Range(-randomX,randomX), Random.Range(-randomY, randomY), 0), transform.rotation, transform);
    }
}
