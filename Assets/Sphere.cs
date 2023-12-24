using DG.Tweening;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Transform centerObject1; // the object to orbit around
    public Transform centerObject2; // the object to move to and collide with
    public float orbitRadius = 2f; // the radius of the orbit
    public float orbitDuration = 1f; // time taken to complete one orbit
    public float orbitSpeed = 1f; // speed of the orbit, set through inspector

    private void Start()
    {
        // Create a new Sequence
        Sequence mySequence = DOTween.Sequence();

        // Calculate the total angle to rotate around the centerObject1 in 2-3 rounds
        float totalAngle = 360f * 3;

        mySequence.AppendCallback(() => StartOrbit(totalAngle, 2f * Mathf.PI * 3 / (orbitDuration / orbitSpeed)))
                  .AppendInterval(orbitDuration * 3)
                  .AppendCallback(MoveToSecondObject);
    }

    void UpdateOrbit(float angle)
    {
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * orbitRadius;
        transform.position = centerObject1.position + offset;
    }

    void StartOrbit(float totalAngle, float speed)
    {
        float currentAngle = 0;
        DOTween.To(() => currentAngle, x => currentAngle = x, totalAngle, totalAngle / speed)
            .OnUpdate(() => UpdateOrbit(currentAngle * Mathf.Deg2Rad));
    }

    void MoveToSecondObject()
    {
        // Move towards the second object
        transform.DOMove(centerObject2.position, 1f).OnComplete(() =>
        {
            Destroy(gameObject);
            // TODO: Handle collision and destruction
        });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == centerObject2)
        {
            Destroy(gameObject);
        }
    }
}