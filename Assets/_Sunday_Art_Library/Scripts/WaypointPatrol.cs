using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    Vector3 startPosition;
    
    Transform targetWaypoint;
    int pathNodeIndex = 0;
    bool followsPath = false;

    [SerializeField] private GameObject agent = null;
    [SerializeField] private GameObject pathWaypoints = null;
    [SerializeField] private bool destroyAtGoal = false;
    [SerializeField] private bool restartPosition = false;
    [SerializeField] private float startDelay = 0;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = agent.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DelayStartTime(startDelay));

        if (followsPath)
        {
            //Debug.Log("Follow Path");
            if (targetWaypoint == null)
            {
                GetNextWaypoint();
                if (targetWaypoint == null)
                {
                    ReachedGoal();
                    return;
                }
            }

            Vector3 dir = targetWaypoint.position - agent.transform.position;
            float distThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame)
            {
                targetWaypoint = null;
            }
            else
            {
                agent.transform.Translate(dir.normalized * distThisFrame, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    IEnumerator DelayStartTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        followsPath = true;
    }

    void GetNextWaypoint()
    {
        if (pathNodeIndex < pathWaypoints.transform.childCount)
        {
            targetWaypoint = pathWaypoints.transform.GetChild(pathNodeIndex);
            pathNodeIndex++;
        }
        else
        {
            targetWaypoint = null;
            ReachedGoal();
            //Debug.Log("Goal Reached");
        }
    }

    private void ReachedGoal()
    {
        //Debug.Log("Last WP");
        if (restartPosition)
        {
            agent.transform.position = startPosition;
            pathNodeIndex = 0;
            //Debug.Log("Restart WP");
        }

        if (destroyAtGoal)
        {
            Destroy(gameObject);
        }

    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        /*
        for (int i = 0; i < pathNodeIndex; i++)
        {
            if (i + 1 >= pathNodeIndex)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pathWaypoints.transform.GetChild(i).transform.position, pathWaypoints.transform.GetChild(i + 1).transform.position);
        }
        */
    }

}
