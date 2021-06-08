using GXPEngine;
using System.Collections.Generic;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class SufficientAgent : NodeGraphAgent
{
	//Current target to move towards
	private Node _target = null;

	private readonly List<Node> NodesMoveList;

	public SufficientAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		if (pNodeGraph.nodes.Count > 0)
		{
			jumpToNode(pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)]);
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		NodesMoveList.Add(pNode);
            /*_target = pNode;*/
        
	}

	protected override void Update()
	{
		//no target? Don't walk
		if (_target == null) return;

		//Move towards the target node, if we reached it, clear the target
		if (moveTowardsNode(_target))
		{
			NodesMoveList.RemoveAt(0);
			if (NodesMoveList.Count > 0)
			{
				_target = NodesMoveList[0];
			}
			else return;
			/*_target = null;*/
		}
	}
}
