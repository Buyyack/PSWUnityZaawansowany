using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Player player;
    public Vector3 currentDestination;

    public int hp = 100;
    public int dmg = 20;

    public float atkRange = 2f;
    public float speed = 5f;
    public float radius = 1f;
    public float lineOfSight = 60f;
    public float detectionDistance = 100f;

    bool isDead = false;
    bool isChasing = false;
    bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Player has not been assigned!");
        GetNewDestination();
    }

    void GetNewDestination()
    {
        currentDestination = new Vector3(Random.value * 50f - 25f, 0f, Random.value * 50f - 25f);
        isChasing = false;
        StopCoroutine(MoveToDestination());
        StartCoroutine(MoveToDestination());
    }

    void CheckLineOfSight()
    {
        Vector3 targetDirection = player.transform.position - transform.position;

        //Angle math
        float dot = Vector3.Dot(targetDirection.normalized, transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < lineOfSight && angle > -lineOfSight)
            CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionDistance)
        {
            isChasing = true;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{this.name} has taken {damage} damage!");
        if (hp <= 0)
            isDead = true;
    }

    IEnumerator MoveToDestination()
    {
        while (Vector3.Distance(transform.position, currentDestination) > (radius))
        {
            if (!isChasing)
            {
                transform.LookAt(currentDestination);
                transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);
                CheckLineOfSight();
                yield return new WaitForEndOfFrame();
            }
            else
            {
                transform.LookAt(player.transform.position);
                if (Vector3.Distance(transform.position, currentDestination) <= atkRange)
                {
                    isChasing = false;
                    // Attack
                    if (canAttack)
                        StartCoroutine(Attack());
                }
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        
        GetNewDestination();
    }

    IEnumerator Attack()
    {
        canAttack = false;
        player.TakeDamage(dmg);
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }
}
