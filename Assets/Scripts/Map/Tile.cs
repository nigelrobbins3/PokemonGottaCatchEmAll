using UnityEngine;

public class Tile
{
	private bool isWalkable;
	private bool isOccupied;
	private Vector2[] neighbors = new Vector2[4];
	public int type;

	public Tile(int type, Vector2 coords){
		this.type = type;
		if (type == 0) {
			// type 0 is a walkable tile from every direction
			neighbors [0] = new Vector2 (coords.x, coords.y + 1); //N
			neighbors [1] = new Vector2 (coords.x - 1, coords.y); //W
			neighbors [2] = new Vector2 (coords.x, coords.y - 1); //S
			neighbors [3] = new Vector2 (coords.x + 1, coords.y); //E
			isWalkable = true;
		} else if (type == -1) {
			// type -1 is an out-of-bounds type since I can't use null
			for (int i = 0; i < 4; i++)
				neighbors [i] = Vector2.zero;
			isWalkable = false;
		}
		isOccupied = false;
	}

	public bool MoveIntoTileFrom(Vector2 source){
		if (isWalkable && (source == neighbors [0] || source == neighbors [1] || source == neighbors [2] || source == neighbors [3])) {
			//the tile is walkable and can be reached from source
			if (!isOccupied){
				isOccupied = true;
				return true; // occupy tile and tell caller that move suceeded
			}
		}
		return false; // not all the condidtions were met. Move failed.
	}

	public void MoveOut(){
		isOccupied = false;
	}

	public bool IsOutOfBounds(){
		return (type == -1);
	}
}
