using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    [Header("Hover Loop")]
    public AnimationCurve hoverCurve = AnimationCurve.EaseInOut(0, 0, 1, 0.5f);
    public WrapMode wrapMode = WrapMode.PingPong;
    public float hoverHeight = 0;
    [Range(1, 5)]
    public int hoverSpeed = 1;
    [Space(10)]
    public bool doHover = true;


    private void Start()
    {
        hoverHeight = hoverHeight + transform.localPosition.y;
        ChangeWrapMode();
    }

    void Update()
    {
        if(doHover)
            Hover();
    }

    public void Hover()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, hoverHeight + hoverCurve.Evaluate((Time.time % hoverCurve.length) * hoverSpeed), transform.localPosition.z);//set on ping pong
    }

    public void ChangeWrapMode()
    {
        hoverCurve.preWrapMode = wrapMode;
        hoverCurve.postWrapMode = wrapMode;
    }
}
