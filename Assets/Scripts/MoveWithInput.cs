using UnityEngine;
using System.Collections;

public class MoveWithInput : MonoBehaviour {

	public float speed;

	private Vector3 targetTile; // target location (just location if not moving)
	private Vector3 velocity; // declaration of movement direction
	private float beginMovementTime;

	void Start () {
		targetTile = transform.position;
		velocity = new Vector3(0, 0, 0);
		speed = 2;
	}

	Vector3 getInputAsVector(){
		Vector3 direction = Vector3.zero;
		if (Input.GetButton ("rightKB")) direction.x += 1;
		if (Input.GetButton ("leftKB")) direction.x -= 1;
		if (Input.GetButton ("upKB")) direction.y += 1;
		if (Input.GetButton ("downKB")) direction.y -= 1;
		return direction;
	}

	void Update () {
		// if the gameObject is on the tile it's supposed to be, start a new movement with new inputs.
		if (transform.position == targetTile) {
			Vector3 directionVector = getInputAsVector ();

			// I like this movement pattern. If the game freezes for a few frames, then start movement from 0;
			// If the game is running smoothly, add an imaginary buffer frame so the first movement frame is at T+1.
			beginMovementTime = Time.deltaTime > 0.05 ? Time.time : Time.time - Time.deltaTime;

			if (directionVector == Vector3.zero) {
				velocity = Vector3.zero; // don't move without input
			} 
			else if (Vector3.Dot(velocity, directionVector) <= 0) { 
				// the input direction(s) is definitely different from the current velocity (which may be zero). Change velocity.
				if (directionVector.y > 0) velocity = Vector3.up;
				else if (directionVector.x > 0) velocity = Vector3.right;
				else if (directionVector.y < 0) velocity = Vector3.down;
				else if (directionVector.x < 0) velocity = Vector3.left;
			}
			// otherwise there is a component in the direction of travel, which is dominant.
			// we are on a tile, go to the tile targeted by velocity
			RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity, 1); // this has a depth property which could be used for bridges???
			if (hit.collider != null)
				velocity = Vector3.zero;
			targetTile += velocity;
		}

		transform.position = Vector3.Lerp(targetTile - velocity, targetTile, (Time.time - beginMovementTime) * speed);
		// targetTile - velocity is the old tile we are interpolating away from. 
	}
}
