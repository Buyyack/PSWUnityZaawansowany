using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    public Player player;
    [SerializeField]
    AStarUser aStarPathfinding;
    [SerializeField]
    AStar aStar;
    public int neighboursDepth = 4;
    public GridGenerator gridGen;

    public int hp = 100;
    public int dmg = 20;

    public int searchDistance = 15;

    //Ex-Combat variables
    public float atkRange = 2f;
    public float speed = 5f;
    public float radius = 1f;
    public float lineOfSight = 60f;
    public float detectionDistance = 100f;
    bool isDead = false;
    bool isChasing = false;
    bool canAttack = true;

    [SerializeField]
    bool isHiding = false;
    public bool isHidden = false;
    Node[] neighbours;

    public Node currentDestination = null;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Player has not been assigned!");
        aStar = FindObjectOfType<AStar>();
        aStarPathfinding = GetComponent<AStarUser>();
        gridGen = FindObjectOfType<GridGenerator>();
    }

    private void Update()
    {
        isHiding = aStarPathfinding.isMoving;
        if (!isHidden && !isHiding)
        {
            //Scan for nearby nodes to hide.
            /*Node nextNode = CheckNearbyNodes();
            
            if (nextNode != null)
            {
                aStarPathfinding.SetDestination(nextNode.worldPosition);
            }
            */
            aStarPathfinding.SetDestination(CheckForBetterPosition().worldPosition);
        }
    }

    Node CheckNearbyNodes()
    {
        neighbours = aStar.Grid.GetNeighbours(aStar.Grid.NodeFromWorldPoint(transform.position), neighboursDepth).ToArray();

        Vector3 playerPos = player.transform.position;
        Node farthestNode = null;
        Node hiddenNode = null;
        float maxDst = Vector3.Distance(playerPos, transform.position);

        for (int i = 0;  i < neighbours.Length; i++)
        {
            Node node = neighbours[i];

            if (node.walkable)
            {
                float currNodeDst = Vector3.Distance(playerPos, node.worldPosition);

                //Debug.DrawRay(node.worldPosition, playerPos - node.worldPosition, Color.red, .1f);
                Ray ray = new Ray(node.worldPosition, playerPos - node.worldPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50f))
                {
                    if (hit.collider.gameObject != player.gameObject)
                    {
                        Debug.DrawRay(node.worldPosition, playerPos - node.worldPosition, Color.red, .5f);
                        hiddenNode = node;
                    }
                }

                maxDst = Mathf.Max(maxDst, currNodeDst);
                if (maxDst == currNodeDst)
                    farthestNode = node;
            }
        }
        if (hiddenNode != null)
            return hiddenNode;
        //There is nowhere to run ]:->
        return farthestNode;
    }

    List<Node> possibleHidingSpots = new List<Node>();
    void FindObstacleBorders()
    {
        List<Node> nodes = gridGen.NonWalkableNodes;

        foreach (Node node in nodes)
        {
            foreach (Node n in gridGen.GetNeighbours(node, false))
            {
                if (!possibleHidingSpots.Contains(n))
                {
                    possibleHidingSpots.Add(n);
                }
            }
        }
    }

    Node CheckForBetterPosition()
    {
        if (possibleHidingSpots.Count < 1)
            FindObstacleBorders();
        Node bestHidingSpot = null;
        float minDst = searchDistance;
        float maxDst = 0f;

        foreach (Node node in possibleHidingSpots)
        {
            Ray ray = new Ray(node.worldPosition, player.transform.position - node.worldPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.gameObject != player.gameObject)
                {
                    float distanceFromAI = Vector3.Distance(transform.position, node.worldPosition);
                    float distanceFromPlayer = Vector3.Distance(player.transform.position, node.worldPosition);
                    if (Mathf.Abs(distanceFromAI - minDst) < searchDistance && distanceFromPlayer > maxDst)
                    {
                        minDst = distanceFromAI;
                        maxDst = distanceFromPlayer;
                        bestHidingSpot = node;
                    }
                }
            }

        }

        currentDestination = bestHidingSpot;
        return bestHidingSpot;
    }

    private void OnDrawGizmos()
    {
        if (neighbours != null && neighbours.Length > 0)
        {
            foreach (Node node in neighbours)
            {
                Gizmos.DrawSphere(node.worldPosition, .5f);
            }
        }

        if (currentDestination != null)
            Gizmos.DrawLine(transform.position, currentDestination.worldPosition);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{this.name} has taken {damage} damage!");
        if (hp <= 0)
            isDead = true;
    }
}
