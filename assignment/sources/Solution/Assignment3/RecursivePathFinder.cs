using GXPEngine;
using System;
using System.Collections.Generic;

/**
 * An example of a PathFinder implementation which completes the process by rolling a die 
 * and just returning the straight-as-the-crow-flies path if you roll a 6 ;). 
 */
class RecursivePathFinder : PathFinder
{

    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) { }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        return MakePath(pFrom, pTo, new List<Node>(), pFrom) ;
    }

    private List<Node> MakePath(Node position, Node target, List<Node> pPath, Node previousNode)
    {
        List<Node> path = pPath;
        path.Add(position);

        Queue<Node> possibleConnections = new Queue<Node>();
        foreach (Node connection in position.connections)
        {
            if (!path.Contains(connection)) possibleConnections.Enqueue(connection);
        }

        Console.WriteLine("Node: " +position.id+  ", possible connections: " + possibleConnections.Count);
        if (position.connections.Contains(target))
        {
            path.Add(target);
            return path;
        }
        else if (possibleConnections.Count > 0)
        {
            return MakePath(possibleConnections.Dequeue(), target, path, position);
        }
        else return MakePath(position, target, path, position);
        Console.WriteLine("No Path Possible"); 
    }
}

