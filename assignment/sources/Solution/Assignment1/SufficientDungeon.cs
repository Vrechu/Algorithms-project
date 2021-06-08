using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

/**
 * An example of a dungeon implementation.  
 * This implementation places two rooms manually but your implementation has to do it procedurally.
 */
class SufficientDungeon : Dungeon
{
	private readonly List<Room> openRooms = new List<Room>();
	private readonly List<Room> newRooms = new List<Room>();

	readonly private int _dungeonWidth = 40;
	readonly private int _dungeonHeight = 30;

	readonly private int _minimumDoorWallDistance = 1;


	public SufficientDungeon(Size pSize) : base(pSize) { }

	/**
	 * This method overrides the super class generate method to implement a two-room dungeon with a single door.
	 * The good news is, it's big enough to house an Ogre and his ugly children, the bad news your implementation
	 * should generate the dungeon procedurally, respecting the pMinimumRoomSize.
	 * 
	 * Hints/tips: 
	 * - start by generating random rooms in your own Dungeon class and placing random doors.
	 * - playing/experiment freely is the key to all success
	 * - this problem can be solved both iteratively or recursively
	 */
	protected override void generate(int pMinimumRoomSize)
	{
		Room startingRoom = new Room(0, 0, _dungeonWidth, _dungeonHeight, pMinimumRoomSize);
		openRooms.Add(startingRoom);
		DivideRooms(pMinimumRoomSize);
		Console.WriteLine("Connecting rooms...");
		ConnectRooms(startingRoom);
		Console.WriteLine("Room connection done");


		//left room from 0 to half of screen + 1 (so that the walls overlap with the right room)
		//(TODO: experiment with removing the +1 below to see what happens with the walls)
		//rooms.Add(new Room(new Rectangle(0, 0, size.Width / 2 + 1, size.Height)));
		//right room from half of screen to the end
		//rooms.Add(new Room(new Rectangle(size.Width / 2, 0, size.Width / 2, size.Height)));
		//and a door in the middle wall with a random y position
		//TODO:experiment with changing the location and the Pens.White below
		//doors.Add(new Door(new Point(size.Width / 2, size.Height / 2 + Utils.Random(-5, 5))));
	}

	#region room division
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// randomly splits the dungeon into multiple smaller rooms of random sizes higher than the minimum size.
	/// </summary>
	/// <param name="minimumRoomSize">minimum size a room side can be</param>
	private void DivideRooms(int minimumRoomSize)
	{
		Console.WriteLine("Dividing rooms...");
		int currentLoop = 0;
		while (openRooms.Count > 0)
		{
			SplitRooms(minimumRoomSize);
			MoveNewRoomsToOpenRooms();
			draw();
			currentLoop++;
			Console.WriteLine("Loop: " + currentLoop + ", Open rooms: " + openRooms.Count + ", Closed rooms: " 
				+ closedRooms.Count + ", Total rooms: " + (openRooms.Count + closedRooms.Count));
		}
		Console.WriteLine("Room division done");
	}

	/// <summary>
	/// checks whether rooms should be split, and whether they should be split horizontally or vertically. 
	/// Adds non-split rooms to the closedRooms list.
	/// </summary>
	/// <param name="minimumRoomSize">minimum size a room side can be</param>
	private void SplitRooms(int minimumRoomSize)
	{
		foreach (Room openRoom in openRooms)
		{
			switch (openRoom.roomType)
			{
				case Room.RoomType.VERTICALSPLIT:
					{
						SplitRoomVertically(openRoom, minimumRoomSize);
						break;
					}
				case Room.RoomType.HORIZONTALSPLIT:
					{
						SplitRoomHorizontally(openRoom, minimumRoomSize);
						break;
					}
				case Room.RoomType.CLOSEDROOM:
					{
						closedRooms.Add(openRoom);
						break;
					}
			}
		}
		openRooms.Clear();
	}

	/// <summary>
	/// splits the room vertically by creating two horizontally neighboring child rooms of random widths that add up to the width of the parent room.
	/// </summary>
	/// <param name="parentRoom">room that should be split</param>
	/// <param name="minimumRoomSize">the minimum size a room side should be</param>
	private void SplitRoomVertically(Room parentRoom, int minimumRoomSize)
	{
		int randomWidth = Utils.Random(minimumRoomSize, parentRoom.width - minimumRoomSize);
		newRooms.Add(parentRoom.childRoomA = new Room(parentRoom.x, parentRoom.y, randomWidth + 1, parentRoom.height, minimumRoomSize));
		newRooms.Add(parentRoom.childRoomB = new Room(parentRoom.x + randomWidth, parentRoom.y, parentRoom.width - randomWidth, parentRoom.height, minimumRoomSize));
	}

	/// <summary>
	/// splits the room horizontally by creating two vertically neighboring child rooms of random heights that add up to the width of the parent room.
	/// </summary>
	/// <param name="parentRoom">room that should be split</param>
	/// <param name="minimumRoomSize">the minimum size a room side should be</param>
	private void SplitRoomHorizontally(Room parentRoom, int minimumRoomSize)
	{
		int randomHeight = Utils.Random(minimumRoomSize, parentRoom.height - minimumRoomSize);
		newRooms.Add(parentRoom.childRoomA = new Room(parentRoom.x, parentRoom.y, parentRoom.width, randomHeight + 1, minimumRoomSize));
		newRooms.Add(parentRoom.childRoomB = new Room(parentRoom.x, parentRoom.y + randomHeight, parentRoom.width, parentRoom.height - randomHeight, minimumRoomSize));
	}

	/// <summary>
	/// moves all the rooms in newRooms to openRooms
	/// </summary>
	private void MoveNewRoomsToOpenRooms()
	{
		foreach (Room room in newRooms)
		{
			openRooms.Add(room);
		}
		newRooms.Clear();
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	#endregion

	#region room connection
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// connects all the rooms in the dungeonwith at least one neighboring room using the minimum amount of doors
	/// </summary>
	/// <param name="parentRoom">starting parent room</param>
	private void ConnectRooms(Room parentRoom)
	{		
		if (parentRoom.roomType != Room.RoomType.CLOSEDROOM)
        {
			ConnectChildRooms(parentRoom);
			ConnectRooms(parentRoom.childRoomA);
			ConnectRooms(parentRoom.childRoomB);
        }
	}

	/// <summary>
	/// places a door between the lowest childrooms of determined parentroom
	/// </summary>
	/// <param name="parentRoom">room of which the childrooms should be connected</param>
	private void ConnectChildRooms(Room parentRoom)
	{
		Room roomA = parentRoom.childRoomA.ClosestConnectingChildRoom(parentRoom.childRoomB);
		Room roomB = parentRoom.childRoomB.ClosestConnectingChildRoom(roomA);

		if (AlgorithmsAssignment.DistanceSquared(roomA.widthMiddle, roomB.widthMiddle) >
			AlgorithmsAssignment.DistanceSquared(roomA.heightMiddle, roomB.heightMiddle))
		{
			ConnectSidewaysRooms(roomA, roomB);
		}
		else
		{
			ConnectStackingRooms(roomA, roomB);
		}
	}

	/// <summary>
	/// places a door between two rooms that are next to eachother
	/// </summary>
	/// <param name="roomA"></param>
	/// <param name="roomB"></param>
	private void ConnectSidewaysRooms(Room roomA, Room roomB)
    {
		//Console.WriteLine("sideways");
		Door newDoor;
		int lowestpoint = AlgorithmsAssignment.Highest(roomA.y, roomB.y);
		int highestPoint = AlgorithmsAssignment.Lowest(roomA.y + roomA.height, roomB.y + roomB.height);
		if (roomA.x < roomB.x)
		{
			doors.Add(newDoor = new Door(new Point(roomA.x + roomA.width - 1,
				Utils.Random(lowestpoint + _minimumDoorWallDistance, highestPoint - _minimumDoorWallDistance))));
		}
		else
		{
			doors.Add(newDoor = new Door(new Point(roomB.x + roomB.width - 1,
				Utils.Random(lowestpoint + _minimumDoorWallDistance, highestPoint - _minimumDoorWallDistance))));
		}
		newDoor.roomA = roomA;
		newDoor.roomB = roomB;
	}

	/// <summary>
	/// places a door between two rooms that that are on top of eachother
	/// </summary>
	/// <param name="roomA"></param>
	/// <param name="roomB"></param>
	private void ConnectStackingRooms(Room roomA, Room roomB)
    {
		//Console.WriteLine("stacking");
		Door newDoor;
		int lowestPoint = AlgorithmsAssignment.Highest(roomA.x, roomB.x);
		int highestPoint = AlgorithmsAssignment.Lowest(roomA.x + roomA.width, roomB.x + roomB.width);
		if (roomA.y < roomB.y)
		{
			doors.Add(newDoor = new Door(new Point(Utils.Random(lowestPoint + _minimumDoorWallDistance, highestPoint - _minimumDoorWallDistance),
				roomA.y + roomA.height - 1)));
		}
		else
		{
			doors.Add(newDoor = new Door(new Point(Utils.Random(lowestPoint + _minimumDoorWallDistance, highestPoint - _minimumDoorWallDistance),
				roomB.y + roomB.height - 1)));
		}
		newDoor.roomA = roomA;
		newDoor.roomB = roomB;
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	#endregion
}

