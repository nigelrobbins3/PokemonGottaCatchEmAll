using UnityEngine;

public static class LocalMap 
{
	public static int mapWidth = 10;
	public static int mapHeight = 10;
	private static int originOffsetX;
	private static int originOffsetY;

	private static Tile[][] map;
	// this is purely for Unity organizing, otherwise the hierarchy is spammed with tiles
	private static GameObject TileHolder = new GameObject ("Tiles");

	public static void LoadMap(Vector2 coords){
		map = new Tile[mapWidth][];
		for (int i = 0; i < mapWidth; i++)
			map [i] = new Tile[mapHeight];
		originOffsetX = mapWidth / 2; // I do this here because I don't know why.
		originOffsetY = mapHeight / 2;

		//for now stub by setting map to always equal the same thing
		for (int x=0; x<mapWidth; x++){
			for (int y=0; y<mapHeight; y++) {
				map [x] [y] = new Tile (0, new Vector2(x - originOffsetX, y - originOffsetY));
			}
		}
	}

	public static Tile GetTile(Vector2 coords){
		int x = (int)coords.x + originOffsetX;
		int y = (int)coords.y + originOffsetY;
		if (x < 0 || x > mapWidth || y < 0 || y > mapHeight)
			return new Tile(-1, Vector2.zero); // Out of bounds
		return map[x][y];
	}

	public static void RenderMap(){
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapHeight; y++) {
				switch (map [x] [y].type) {
				case 0:
					Object.Instantiate (
						Resources.Load("Tiles/PathSprite"), 
						new Vector3 (x - originOffsetX, y - originOffsetY, 0f), 
						Quaternion.identity,
						TileHolder.transform
					);
					break;
				}
			}
		} 
	}
}
