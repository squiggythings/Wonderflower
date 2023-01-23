using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour
{
    public Vector2 startSpeed;
    public Vector2 gravity;
    public Vector2 friction;
    [Space]
    public Vector2 startSpeedRandomization;
    public Vector2 gravityRandomization;
    public Vector2 frictionRandomization;

    private Vector2 speed;
    private Vector2 position;

    private void Start()
    {
        position.x = transform.position.x;
        position.y = transform.position.y;

        speed.x = startSpeed.x;
        speed.y = startSpeed.y;
        speed.x += Random.Range(-startSpeedRandomization.x, startSpeedRandomization.x);
        speed.y += Random.Range(-startSpeedRandomization.y, startSpeedRandomization.y);

        gravity.x += Random.Range(-gravityRandomization.x, gravityRandomization.x);
        gravity.y += Random.Range(-gravityRandomization.y, gravityRandomization.y);

        friction.x += Random.Range(-frictionRandomization.x, frictionRandomization.x);
        friction.y += Random.Range(-frictionRandomization.y, frictionRandomization.y);
    }

    private void Update()
    {
        speed += gravity;
        speed *= friction;
        position += speed;

        transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
    }
}
