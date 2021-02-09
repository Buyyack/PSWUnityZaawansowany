using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChecker : MonoBehaviour
{
    public Target target;
    public float radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            Debug.LogError("No target assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CheckAngle();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < (radius + target.radius))
            Debug.Log($"Collided with {target.name}!");
    }

    public void CheckAngle()
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        //Angle math
        float dot = Vector3.Dot(targetDirection.normalized, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Direction math
        float targetVerticalPosition = Vector3.Cross(transform.forward, targetDirection).y;
        float targetHorizontalPosition = Vector3.Cross(transform.forward, targetDirection).x;
        Debug.Log($"vert {targetVerticalPosition}, hor {targetHorizontalPosition}");

        string direction = "";

        if (targetHorizontalPosition > 0)
            direction += "Forward-";
        else if (targetHorizontalPosition < 0)
            direction += "Behind-";

        if (targetVerticalPosition > 0)
            direction += "Right";
        else if (targetVerticalPosition < 0)
            direction += "Left";

        Debug.Log($"Target location is: {direction} and the angle between player forward and target = {angle}");
    }
}
