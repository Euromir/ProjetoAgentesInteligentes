using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Node start;
    public Node destination;
    public List<Node> tiles = new List<Node>();

    void Start()
    {
        var nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        tiles = nodes.ToList();

        SetStartNodeFromScene();

        AddAllNeighbours();

    }

    void SetStartNodeFromScene()
    {
        foreach (Node node in tiles)
        {
            if (node.isStart)
            {
                start = node;
                break;
            }

            if (start == null && node.name == "hex_sand (14)")
            {
                start = node;
                start.isStart = true;
            }
        }
        Debug.Log("Start node set to " + start.name);
    }
    void AddAllNeighbours()
    {

        foreach (Node tile in tiles)
        {
            tile.neighbours.Clear();
        }

        foreach (Node tile in tiles)
        {
            foreach (Node other in tiles)
            {
                if (tile != other && Vector3.Distance(tile.gridPosition, other.gridPosition) <= 1.1f)
                {
                    tile.neighbours.Add(other);
                }
            }
        }
        Debug.Log("All neighbours added.");
    }

    public void SetNewStart(Node newStart)
    {
        if (start != null)
            start.isStart = false;

        start = newStart;
        start.isStart = true;
    }

    public void SetNewDestination(Node newDestination)
    {
        if (newDestination.isObstacle)
        {
            Debug.Log("Não é possível definir um obstáculo como destino.");
            return;
        }

        destination = newDestination;
        Debug.Log("Destino definido para " + newDestination.name);
    }
}