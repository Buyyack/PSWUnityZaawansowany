using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EntityAI[] targets;

    public int hp = 100;
    public int dmg = 20;
    
    public float atkRange = 2f;
    public float radius = 1f;
    public float atkCoolDown = 2f;

    float horizontal, vertical;
    public float speed = 10f;

    bool hasCollided = false;
    bool canAttack = true;
    bool isDead = true;

    // Start is called before the first frame update
    void Start()
    {
        targets = FindObjectsOfType<EntityAI>();
        if (targets == null)
            Debug.LogError("No target assigned!");
    }

    private void FixedUpdate()
    {
        if (targets.Length > 0)
        {
            foreach (EntityAI target in targets)
            {
                Debug.DrawRay(transform.position, target.transform.position - transform.position);
                Ray ray = new Ray(transform.position, target.transform.position - transform.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.collider.gameObject == target.gameObject)
                    {
                        target.isHidden = false;
                    }
                    else
                    {
                        target.isHidden = true;
                    }
                    //print($"Raycast hit {hit.collider.gameObject.name}");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;
        /*
        if (Input.GetKeyDown(KeyCode.E))
            CheckAngle();

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealDamage(dist);
        }

        if (dist < (radius + target.radius))
        {
            hasCollided = true;
        }

        */
    }

    /*
    void DealDamage(float dist)
    {
        if (hasCollided && canAttack && dist <= atkRange)
            StartCoroutine(Attack(atkCoolDown));
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{this.name} has taken {damage} damage!");
        if (hp <= 0)
            isDead = true;
    }

    IEnumerator Attack(float cooldown)
    {
        //Disable ability to attack, deal damage, wait for 2 seconds, enable ability to attack.
        target.TakeDamage(dmg);
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    public void CheckAngle()
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        //Angle math
        float dot = Vector3.Dot(targetDirection.normalized, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Direction math
        Vector3 directionCross = Vector3.Cross(transform.forward, targetDirection);
        float targetVerticalPosition = directionCross.y;
        float targetHorizontalPosition = directionCross.x;

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
    */
}
