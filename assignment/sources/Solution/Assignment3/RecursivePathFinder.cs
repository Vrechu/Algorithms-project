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

    private List<Node> ShortestPath;
    private List<Node> VisitedNodes;

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        //at this point you know the FROM and TO node and you have to write an 
        //algorithm which finds the path between them
        Console.WriteLine("For now I'll just roll a die for a random path!!");

        int dieRoll = Utils.Random(1, 7);
        Console.WriteLine("I rolled a ..." + dieRoll);

        if (dieRoll == 6)
        {
            Console.WriteLine("Yes 'random' path found!!");
            return new List<Node>() { pFrom, pTo };
        }
        else
        {
            Console.WriteLine("Too bad, no path found !!");
            return null;
        }
    }

    private List<Node> MakePath(Node position, Node target, List<Node> pPath)
    {
        List<Node> path = pPath;
        path.Add(position);


        List<Node> possibleConnections = new List<Node>();
        foreach (Node connection in position.connections)
        {
            if (!path.Contains(connection)) possibleConnections.Add(connection);
        }

        if (position.connections.Contains(target))
        {
            path.Add(target);
            return path;
        }

        else if (possibleConnections != null)
        {
            List<Node> possiblePath = new List<Node>();
            for (int i = 0; path.FindLast(Node) == ; i++)
            {
                possiblePath = 
            }
        }

        else
        {
            Console.WriteLine("No Possible path!");
            return null;

        }

    }
}

