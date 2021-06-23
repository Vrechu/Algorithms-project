using GXPEngine;
using System;
using System.Collections.Generic;

class BFSPathFinder : PathFinder
{
    public BFSPathFinder(NodeGraph pGraph) : base(pGraph) { }

    private List<List<Node>> _paths = new List<List<Node>>();
    private Queue<List<Node>> _openPaths = new Queue<List<Node>>();

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        _paths.Clear();        
        MakePaths(pFrom, pTo, new List<Node>());
        /*_paths.Sort();*/
        return _paths[0];
    }

    private void MakePaths(Node position, Node target, List<Node> pPath)
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
            _paths.Add(path);
        }

        else if (_openPaths.Count > 0)
        {
            while (_openPaths.Count > 0)
            {
                MakePaths(_openPaths.Peek()[_openPaths.Peek().Count-1], target, _openPaths.Dequeue());
            }
        }

        else Console.WriteLine("Dead end!");
    }

    private List<Node> PathIncludingNode(List<Node> pPath, Node node)
    {
        List<Node> path = new List<Node>();
        path.AddRange(pPath);
        path.Add(node);
        return path;
    }
}

