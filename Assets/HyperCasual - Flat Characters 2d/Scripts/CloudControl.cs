using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControl : MonoBehaviour
{
    public Transform[] points = new Transform[2];

    private float _speed = 0.5f;

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);

        if(transform.position.x > points[1].position.x)
        {
            var randYStep = Random.Range(-2, 2);

            transform.position = new Vector2(points[0].position.x, points[0].position.y + randYStep);
        }
    }
}