using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSway : MonoBehaviour
{
    [Header("Sway")]
    public AnimationCurve swayCurveX = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public AnimationCurve swayCurveY = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public AnimationCurve swayCurveZ = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public WrapMode wrapMode = WrapMode.PingPong;
    public Vector3 swayIntensity = new Vector3(1, 1, 1);
    [Range(1, 5)]
    public int swaySpeed = 1;
    [Space(10)]
    public bool doSway = true;

    private void Start()
    {
        ChangeWrapMode();
    }

    void Update()
    {
        if (doSway)
            Sway();
    }

    public void Sway()
    {
        Quaternion target = Quaternion.Euler(new Vector3(swayIntensity.x * swayCurveX.Evaluate((Time.time % swayCurveX.length) * swaySpeed), swayIntensity.y * swayCurveY.Evaluate((Time.time % swayCurveY.length) * swaySpeed), swayIntensity.z * swayCurveZ.Evaluate((Time.time % swayCurveZ.length) * swaySpeed)));
        transform.localRotation = target;
    }

    public void ChangeWrapMode()
    {
        swayCurveX.preWrapMode = wrapMode;
        swayCurveX.postWrapMode = wrapMode;

        swayCurveY.preWrapMode = wrapMode;
        swayCurveY.postWrapMode = wrapMode;

        swayCurveZ.preWrapMode = wrapMode;
        swayCurveZ.postWrapMode = wrapMode;
    }
}
