using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EnemyAI target;
    
    public int hp = 100;
    public int dmg = 20;
    
    public float atkRange = 2f;
    public float radius = 1f;

    float horizontal, vertical, speed = 10f;

    bool hasCollided = false;
    bool canAttack = false;
    bool isDead = true;

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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hasCollided && canAttack)
                target.TakeDamage(dmg);
        }

        transform.position += new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist < (radius + target.radius))
        {
            hasCollided = true;
            Debug.Log($"Collided with {target.name}!");
        }
        if (dist <= atkRange)
            canAttack = true;
        else
            canAttack = false;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{this.name} has taken {damage} damage!");
        if (hp <= 0)
            isDead = true;
    }

    IEnumerator Attack()
    {
        canAttack = false;
        target.TakeDamage(dmg);
        yield return new WaitForSeconds(2f);
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
}
