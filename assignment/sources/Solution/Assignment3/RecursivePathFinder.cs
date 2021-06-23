using GXPEngine;
using System;
using System.Collections.Generic;

class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) { }

    private List<List<Node>> Paths = new List<List<Node>>();

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        Paths.Clear();
        MakePaths(pFrom, pTo, new List<Node>());
        Paths.Sort();
        return Paths[0];
    }    

    /// <summary>
    /// creates a pathfinding method by exploring every path 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="target"></param>
    /// <param name="pPath"></param>
    private void MakePaths(Node position, Node target, List<Node> pPath)
    {
        List<Node> path = new List<Node>();
        path.AddRange(pPath);
        path.Add(position);
        Queue<Node> possibleConnections = new Queue<Node>();
        foreach (Node connection in position.connections)
        {
            if (!path.Contains(connection)) possibleConnections.Enqueue(connection);
        }

        Console.WriteLine("Node: " + position.id + ", possible connections: " + possibleConnections.Count);
        if (position.connections.Contains(target))
        {
            path.Add(target);
            Console.WriteLine("Path found! length:" + path.Count);
            Paths.Add(path);
        }
        else if (possibleConnections.Count > 0)
        {
            while (possibleConnections.Count > 0)
            {
                MakePaths(possibleConnections.Dequeue(), target, path);
            }
        }
        else Console.WriteLine("Dead end!");
    }
}

