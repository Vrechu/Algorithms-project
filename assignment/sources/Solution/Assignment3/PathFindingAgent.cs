using GXPEngine;
using System;
using System.Collections.Generic;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class PathFindingAgent : NodeGraphAgent
{
	//Current target to move towards
	private Node _targetNode = null;
	private Node _currentNode = null;

	private readonly List<Node> _targetNodes = new List<Node>();

	private PathFinder _pathFinder;
	public PathFindingAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		if (pNodeGraph.nodes.Count > 0)
		{
			_currentNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
			jumpToNode(_currentNode);
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;

		_pathFinder = new BFSPathFinder(pNodeGraph);
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		if (_targetNodes.Count == 0)
		{
			_targetNodes.AddRange(_pathFinder.Generate(_currentNode, pNode));
			_targetNode = _targetNodes[0];
			_currentNode = null;
		}
		else _targetNodes.AddRange(_pathFinder.Generate(_targetNodes[_targetNodes.Count-1], pNode));
	}

	protected override void Update()
	{
		//no target? Don't walk
		if (_targetNode == null) return;

		MoveToNextNodeOnList();

		//if (_targetNode != null) Console.WriteLine(moveTowardsNode(_targetNode));
	}

	/// <summary>
	/// Moves to the next node on the targetNodes list. once the target is reached it is removed from the list and the list is trimmed.
	/// </summary>
	private void MoveToNextNodeOnList()
	{
		if (moveTowardsNode(_targetNode))
		{
			_targetNodes.Remove(_targetNode);
			if (_targetNodes.Count > 0)
			{
				_targetNode = _targetNodes[0];
			}
			else
			{
				_currentNode = _targetNode;
				_targetNode = null;
			}
		}
	}
}
