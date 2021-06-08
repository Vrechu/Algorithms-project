using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
	public Rectangle area;

	readonly public int x;
	readonly public int y;
	readonly public int width;
	readonly public int height;
	readonly public int widthMiddle;
	readonly public int heightMiddle;

	public Node roomNode;

	public enum RoomType
	{
		HORIZONTALSPLIT, VERTICALSPLIT, CLOSEDROOM
	}
	public RoomType roomType = RoomType.CLOSEDROOM;

	public Room childRoomA;
	public Room childRoomB;



	public Room(int px, int py, int pWidth, int pHeight, int pMinimumRoomSize)
	{
		area = new Rectangle(px, py, pWidth, pHeight);

		x = px;
		y = py;
		width = pWidth;
		height = pHeight;
		widthMiddle = x + width / 2;
		heightMiddle = y + height / 2;
		SetRoomType(pMinimumRoomSize);
	}

	/// <summary>
	/// compares the position of the child rooms of this room to the position of the determind room.
	/// </summary>
	/// <param name="roomToConnect">determined room to compare to</param>
	/// <returns>the child room closest to the determined room</returns>
	public Room ClosestConnectingChildRoom(Room roomToConnect)
	{
		switch (roomType)
        {
            default:
                {
					return this;
                }
			case RoomType.VERTICALSPLIT:
                {
					return ClosestConnectingChildRoomHorizontal(roomToConnect);
				}
			case RoomType.HORIZONTALSPLIT:
				{
					return ClosestConnectingChildRoomVertical(roomToConnect);
				}
		}
	}

	/// <summary>
	/// compares the x position of the two child rooms of this room with the determined room
	/// </summary>
	/// <param name="roomToConnect">determined room to compare to</param>
	/// <returns>the child room closest to the determined room</returns>
	private Room ClosestConnectingChildRoomHorizontal(Room roomToConnect)
	{
		if (AlgorithmsAssignment.DistanceSquared(childRoomA.widthMiddle, roomToConnect.widthMiddle) < AlgorithmsAssignment.DistanceSquared(childRoomB.widthMiddle, roomToConnect.widthMiddle))
		{
			return childRoomA.ClosestConnectingChildRoom(roomToConnect);
		}
		else if (AlgorithmsAssignment.DistanceSquared(childRoomA.widthMiddle, roomToConnect.widthMiddle) == AlgorithmsAssignment.DistanceSquared(childRoomB.widthMiddle, roomToConnect.widthMiddle))
		{
			if (Utils.Random(0, 2) > 1) return childRoomA.ClosestConnectingChildRoom(roomToConnect);
			else return childRoomB.ClosestConnectingChildRoom(roomToConnect);
		}
		else return childRoomB.ClosestConnectingChildRoom(roomToConnect);
	}

	/// <summary>
	/// compares the y position of the two child rooms of this room with the determined room
	/// </summary>
	/// <param name="roomToConnect">determined room to compare to</param>
	/// <returns>the child room closest to the determined room</returns>
	private Room ClosestConnectingChildRoomVertical(Room roomToConnect)
	{
		if (AlgorithmsAssignment.DistanceSquared(childRoomA.heightMiddle, roomToConnect.heightMiddle) < AlgorithmsAssignment.DistanceSquared(childRoomB.heightMiddle, roomToConnect.heightMiddle))
		{
			return childRoomA.ClosestConnectingChildRoom(roomToConnect);
		}
		else if (AlgorithmsAssignment.DistanceSquared(childRoomA.heightMiddle, roomToConnect.heightMiddle) == AlgorithmsAssignment.DistanceSquared(childRoomB.heightMiddle, roomToConnect.heightMiddle))
		{
			if (Utils.Random(0, 2) > 1) return childRoomA.ClosestConnectingChildRoom(roomToConnect);
			else return childRoomB.ClosestConnectingChildRoom(roomToConnect);
		}
		else return childRoomB.ClosestConnectingChildRoom(roomToConnect);
	}

	/// <summary>
	/// Checks wether the room should split horizontally, vertically, or not at all.
	/// </summary>
	/// <param name="minimumRoomSize">minimum size a side of the room can be</param>
	private void SetRoomType(int minimumRoomSize)
	{
		if (width / 2 >= minimumRoomSize
		|| height / 2 >= minimumRoomSize)
		{
			if (width >= height)
			{
				roomType = RoomType.VERTICALSPLIT;
				//Console.WriteLine("wide room");
			}
			else if (height > width)
			{
				roomType = RoomType.HORIZONTALSPLIT;
				//Console.WriteLine("tall room");
			}
		}
		else
		{
			roomType = RoomType.CLOSEDROOM;
			//Console.WriteLine("done");
		}
	}

    public override string ToString()
    {
        return base.ToString() + 
			": ("+ x + "-" + (x+width) + ", " + y + "-" + (y + height) + "), " + 
			width + "/" + height +" : "+ roomType;
    }


    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)
}
