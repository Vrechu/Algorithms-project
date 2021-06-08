using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System;

/**
 * An example of a dungeon nodegraph implementation.
 * 
 * This implementation places only three nodes and only works with the SampleDungeon.
 * Your implementation has to do better :).
 * 
 * It is recommended to subclass this class instead of NodeGraph so that you already 
 * have access to the helper methods such as getRoomCenter etc.
 * 
 * TODO:
 * - Create a subclass of this class, and override the generate method, see the generate method below for an example.
 */
class SufficientNodeGraph : NodeGraph
{
	protected Dungeon _dungeon;

	public SufficientNodeGraph(Dungeon pDungeon) : base((int)(pDungeon.size.Width * pDungeon.scale), (int)(pDungeon.size.Height * pDungeon.scale), (int)pDungeon.scale / 3)
	{
		Debug.Assert(pDungeon != null, "Please pass in a dungeon.");

		_dungeon = pDungeon;
	}

	protected override void generate()
	{
		//Generate nodes, in this sample node graph we just add to nodes manually
		//of course in a REAL nodegraph (read:yours), node placement should 
		//be based on the rooms in the dungeon

		//We assume (bad programming practice 1-o-1) there are two rooms in the given dungeon.
		//The getRoomCenter is a convenience method to calculate the screen space center of a room
		//nodes.Add(new Node(getRoomCenter(_dungeon.closedRooms[0])));
		//nodes.Add(new Node(getRoomCenter(_dungeon.closedRooms[1])));
		//The getDoorCenter is a convenience method to calculate the screen space center of a door
		//nodes.Add(new Node(getDoorCenter(_dungeon.doors[0])));

		nodes.AddRange(RoomNodes(_dungeon.closedRooms));
		nodes.AddRange(DoorNodes(_dungeon.doors));
		ConnectNodes();

    }

	/**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given room you can use for your nodes in this class
	 */
	protected Point getRoomCenter(Room pRoom)
	{
		float centerX = ((pRoom.area.Left + pRoom.area.Right) / 2.0f) * _dungeon.scale;
		float centerY = ((pRoom.area.Top + pRoom.area.Bottom) / 2.0f) * _dungeon.scale;
		return new Point((int)centerX, (int)centerY);
	}

	/**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given door you can use for your nodes in this class
	 */
	protected Point getDoorCenter(Door pDoor)
	{
		return getPointCenter(pDoor.location);
	}

	/**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given point you can use for your nodes in this class
	 */
	protected Point getPointCenter(Point pLocation)
	{
		float centerX = (pLocation.X + 0.5f) * _dungeon.scale;
		float centerY = (pLocation.Y + 0.5f) * _dungeon.scale;
		return new Point((int)centerX, (int)centerY);
	}

	/// <summary>
	/// adds nodes to each room in specified room list
	/// </summary>
	/// <param name="doors">room list to add nodes to</param>
	/// <returns></returns>
	private List<Node> RoomNodes(List<Room> rooms)
    {
		List<Node> newNodes = new List<Node>();

		foreach(Room room in rooms)
        {
			newNodes.Add(room.roomNode= new Node(getRoomCenter(room)));
        }
		return newNodes;
    }

	/// <summary>
	/// adds nodes to each door in specified door list
	/// </summary>
	/// <param name="doors">door list to add nodes to</param>
	/// <returns></returns>
	private List<Node> DoorNodes(List<Door> doors)
    {
		List<Node> newNodes = new List<Node>(); 

		foreach(Door door in doors)
        {
			newNodes.Add(door.doorNode= new Node(getDoorCenter(door)));
        }
		return newNodes;
    }

	/// <summary>
	/// connects all nodes to the right neighboring nodes
	/// </summary>
	private void ConnectNodes()
    {
		foreach (Door door in _dungeon.doors)
        {
			AddConnection(door.doorNode, door.roomA.roomNode);
			AddConnection(door.doorNode, door.roomB.roomNode);
        }
    }
}
