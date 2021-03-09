using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarUser : MonoBehaviour
{
    public bool isMoving = false;
    public float speed = 5f;
    Vector3[] path;
    int targetIndex;

    public void SetDestination(Vector3 destination)
    {
        if (destination != null)
            PathRequestManager.RequestPath(transform.position, destination, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess && !isMoving)
        {
            //print("Path found successfuly");
            path = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0 && !isMoving)
        {
            isMoving = true;
            Vector3 currenWaypoint = path[0];

            while (isMoving)
            {
                if (transform.position == currenWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        isMoving = false;
                        yield break;
                    }
                    currenWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currenWaypoint, speed * Time.deltaTime);
                yield return null;
            }
            isMoving = false;
            yield break;
        }
    }
}