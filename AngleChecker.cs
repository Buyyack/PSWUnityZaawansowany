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
        //Angle math
        float dot = Vector3.Dot((target.transform.position - transform.position).normalized, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Direction math
        float targetPosition = Vector3.Cross(transform.forward, target.transform.position - transform.position).y;

        if (targetPosition > 0)
            Debug.Log($"Target is to the right of player, and the angle between player forward and target = {angle}");
        else if (targetPosition < 0)
            Debug.Log($"Target is to the left of player, and the angle between player forward and target = {angle}");
    }
}
