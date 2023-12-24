using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [Header("Constant Rotation")]
    public Vector3 axisRotation;
    public float speedModifier = 1f;
    [Space(10)]
    public bool doRotate = true;

    // Update is called once per frame
    void Update()
    {
        if (doRotate)
            Rotate();
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(transform.rotation.x + axisRotation.x, transform.rotation.y + axisRotation.y, transform.rotation.z + axisRotation.z) * speedModifier * Time.deltaTime);
    }
}
