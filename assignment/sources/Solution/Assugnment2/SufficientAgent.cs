using GXPEngine;
using System;
using System.Collections.Generic;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class SufficientAgent : NodeGraphAgent
{
	//Current target to move towards
	private Node _targetNode = null;
	private Node _currentNode = null;

	private readonly List<Node> targetNodes = new List<Node>();

	public SufficientAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		if (pNodeGraph.nodes.Count > 0)
		{
			_currentNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
			jumpToNode(_currentNode);
			/*jumpToNode(pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)]);*/
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		if (targetNodes.Count > 0 && targetNodes[targetNodes.Count - 1].connections.Contains(pNode)
			|| _currentNode.connections.Contains(pNode))
		{
			targetNodes.Add(pNode);
			Console.WriteLine(pNode);
		_targetNode = targetNodes[0];
		}
        
	}

	protected override void Update()
	{
		//no target? Don't walk
		if (_targetNode == null) return;

		MoveToNextNodeOnList();
	}

	/// <summary>
	/// Moves to the next node on the targetNodes list. once the target is reached it is removed from the list and the list is trimmed.
	/// </summary>
	private void MoveToNextNodeOnList()
    {
		if (moveTowardsNode(_targetNode))
		{
			_currentNode = _targetNode;
			targetNodes.Remove(_targetNode);
			targetNodes.TrimExcess();
			if (targetNodes.Count > 0)
			{
				_targetNode = targetNodes[0];
			}
			else _targetNode = null;
		}
	}
}
