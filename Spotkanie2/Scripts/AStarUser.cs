using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarUser : MonoBehaviour
{
    public Transform target;
    float speed = 20f;
    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            print("NPC has a successful path.");
            path = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        print("NPC is moving to target location...");

        Vector3 currenWaypoint = path[0];

        while (true)
        {
            if (transform.position == currenWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currenWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currenWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}