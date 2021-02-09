using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChecker : MonoBehaviour
{
    public GameObject target;

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

    public void CheckAngle()
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        float dot = Vector3.Dot(targetDirection, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        Debug.Log($"Angle between player forward and target = {angle}");

        Vector3 normalizedDirection = target.transform.position - transform.position;

        float targetPosition = Vector3.Cross(transform.forward, normalizedDirection).y;

        if (targetPosition > 0)
            Debug.Log($"Target is to the right of player.");
        else if (targetPosition < 0)
            Debug.Log($"Target is to the left of player.");
    }
}
