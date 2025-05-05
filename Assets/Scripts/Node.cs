using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 gridPosition;
    public float cost = 1f;
    public float distance = Mathf.Infinity;
    public Node previousNode = null;
    public List<Node> neighbours = new List<Node>();
    public bool visited = false;
    public bool isObstacle = false;
    public bool isStart = false;
    public GameManager gm;

    void Awake()
    {
        gridPosition = transform.position;
    }
    private void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gm != null)
        {
            gm.SetNewDestination(this);
        }
    }

    public void AddNeighbours(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if(Vector3.Distance(node.gridPosition, gridPosition) <= 1.1f)
            {
                if(node == this)
                {
                    continue;
                }
                neighbours.Add(node);
            }
        }
    }
}
