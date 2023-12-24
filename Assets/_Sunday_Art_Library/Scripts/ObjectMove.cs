using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("Move Loop")]
    public AnimationCurve moveCurveX = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public AnimationCurve moveCurveY = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public AnimationCurve moveCurveZ = AnimationCurve.EaseInOut(0, 0, 1, 0);
    public WrapMode wrapMode = WrapMode.PingPong;

    [Range(1, 5)]
    public int moveSpeed = 1;

    [Header("Extra Values")]
    public float amplifyX = 1f;
    public float amplifyY = 1f;
    public float amplifyZ = 1f;

    [Range(1, 10)] public int timeX = 1;
    [Range(1, 10)] public int timeY = 1;
    [Range(1, 10)] public int timeZ = 1;

    [Space(10)]
    public bool doMove = true;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        ChangeWrapMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (doMove)
            MoveMe();
    }

    public void MoveMe()
    {
        transform.localPosition = new Vector3(
            startPos.x + moveCurveX.Evaluate((Time.time / timeX % moveCurveX.length) * moveSpeed) * amplifyX,
            startPos.y + moveCurveY.Evaluate((Time.time / timeY % moveCurveY.length) * moveSpeed) * amplifyY,
            startPos.z + moveCurveZ.Evaluate((Time.time / timeZ % moveCurveZ.length) * moveSpeed) * amplifyZ);
            
    }

    public void ChangeWrapMode()
    {
        moveCurveX.preWrapMode = wrapMode;
        moveCurveX.postWrapMode = wrapMode;

        moveCurveY.preWrapMode = wrapMode;
        moveCurveY.postWrapMode = wrapMode;

        moveCurveZ.preWrapMode = wrapMode;
        moveCurveZ.postWrapMode = wrapMode;
    }

}

