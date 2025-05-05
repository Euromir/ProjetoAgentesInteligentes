using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public List<Node> tiles;
    public List<Node> path = new List<Node>();
    public bool isRunning = false;

    private class NodeRecord
    {
        public Node node;
        public Node connection;
        public float costSoFar;
        public float estimatedTotalCost;

        public NodeRecord(Node node)
        {
            this.node = node;
            this.connection = null;
            this.costSoFar = 0;
            this.estimatedTotalCost = Mathf.Infinity;
        }
    }

    private float Heuristic(Node a, Node b)
    {
        return Vector3.Distance(a.gridPosition, b.gridPosition);
    }

    public void RunAStar(Node startNode, Node destinationNode, List<Node> tiles)
    {
        isRunning = true;
        this.tiles = tiles;

        Dictionary<Node, NodeRecord> nodeRecords = new Dictionary<Node, NodeRecord>();
        List<NodeRecord> open = new List<NodeRecord>();
        List<NodeRecord> closed = new List<NodeRecord>();
        NodeRecord startRecord = new NodeRecord(startNode);

        startRecord.costSoFar = 0;
        startRecord.estimatedTotalCost = Heuristic(startNode, destinationNode);
        open.Add(startRecord);
        nodeRecords[startNode] = startRecord;

        while (open.Count > 0)
        {
            NodeRecord currentRecord = open[0];
            foreach (var record in open)
            {
                if (record.estimatedTotalCost < currentRecord.estimatedTotalCost)
                {
                    currentRecord = record;
                }
            }

            if (currentRecord.node == destinationNode) break;

            foreach (Node neighbour in currentRecord.node.neighbours)
            {
                if (neighbour.isObstacle) continue;

                float endNodeCost = currentRecord.costSoFar + neighbour.cost;
                NodeRecord endNodeRecord;

                if (nodeRecords.ContainsKey(neighbour))
                {
                    endNodeRecord = nodeRecords[neighbour];
                    if (endNodeRecord.costSoFar <= endNodeCost) continue;
                }
                else
                {
                    endNodeRecord = new NodeRecord(neighbour);
                    nodeRecords[neighbour] = endNodeRecord;
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = currentRecord.node;
                endNodeRecord.estimatedTotalCost = endNodeCost + Heuristic(neighbour, destinationNode);

                if (!open.Contains(endNodeRecord))
                    open.Add(endNodeRecord);
            }
            
            open.Remove(currentRecord);
            closed.Add(currentRecord);
        }

        if (nodeRecords.ContainsKey(destinationNode))
        {
            NodeRecord current = nodeRecords[destinationNode];
            path.Clear();
            while (current.node != startNode)
            {
                path.Add(current.node);
                current = nodeRecords[current.connection];
            }
            path.Reverse();
        }

        isRunning = false;
    }
}
