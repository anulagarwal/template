using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "idle", false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "death", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "death", false);
        }

    }
}
