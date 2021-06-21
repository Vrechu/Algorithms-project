using GXPEngine;

/**
 * This is an example subclass of the TiledView that just generates random tiles.
 */
class TiledDungeonView : TiledView
{

	private Dungeon _dungeon;
	/**
	 * This constructor takes a dungeon but doesn't do anything with it, it is just an example of how
	 * to initialize the TiledView parameters with size and scale data from the dungeon,
	 * make sure you understand what is happening here before you continue.
	 */
	public TiledDungeonView(Dungeon pDungeon, TileType pDefaultTileType) : base(pDungeon.size.Width, pDungeon.size.Height, (int)pDungeon.scale, pDefaultTileType)
	{
	}

	/**
	 * Fill the tileview with random data instead.
	 * In your subclass, you should set the tiletype correctly based on the provided dungeon contents.
	 */
	protected override void generate()
	{
		for (int i = 0; i < columns * rows; i++)
		{
			SetTileType(i % columns, i / columns, TileType.GROUND);
		}
	}

	void PlaceWalls()
    {
		foreach (Room room in _dungeon.closedRooms)
        {
			for (int i = 0; i < room.width * room.height; i++)
            {
				SetTileType(i % room.width, i / room.height, TileType.GROUND);
			}
		} 
    }
}

