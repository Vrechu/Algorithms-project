using GXPEngine;
using System;
using System.Collections.Generic;

class BFSPathFinder : PathFinder
{
    public BFSPathFinder(NodeGraph pGraph) : base(pGraph) { }

    private Queue<List<Node>> _openPaths = new Queue<List<Node>>();

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        _openPaths.Clear();
        return MakePaths(pFrom, pTo, new List<Node>()); ;
    }

    /// <summary>
    /// Finds a path on the nodegraph useing BFS
    /// </summary>
    /// <param name="position">node the pathfinder is on</param>
    /// <param name="target">node that the pathfinder wants to reach</param>
    /// <param name="pPath">list of nodes that are required to reach this node</param>
    /// <returns>returns the shortest path from the passed in node to the target node</returns>
    private List<Node> MakePaths(Node position, Node target, List<Node> pPath)
    {
        Console.WriteLine("current node: " + position);
        List<Node> path = new List<Node>();
        path.AddRange(pPath);
        if (!path.Contains(position)) path.Add(position);

        foreach (Node connection in position.connections)
        {
            if (!path.Contains(connection)) _openPaths.Enqueue(PathIncludingNode(path, connection));
        }

        if (position.connections.Contains(target))
        {
            path.Add(target);
            Console.WriteLine("Path found! length:" + path.Count);
            return path; ;
        }

        while (_openPaths.Count > 0)
        {
            return MakePaths(_openPaths.Peek()[_openPaths.Peek().Count - 1], target, _openPaths.Dequeue());
        }
        return null;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pPath">path to add the node to</param>
    /// <param name="node">node to add to the path</param>
    /// <returns>returns a new list of nodes with the specified node at the end</returns>
    private List<Node> PathIncludingNode(List<Node> pPath, Node node)
    {
        List<Node> path = new List<Node>();
        path.AddRange(pPath);
        path.Add(node);
        return path;
    }
}

