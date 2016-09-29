using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 2f;
	private MovableActorBody2 playerBody;

	private Vector3 velocity; // declaration of movement direction

	void Start () {
		velocity = new Vector3(0, 0, 0);
		playerBody = new MovableActorBody2 (transform.position, GetComponent<CircleCollider2D>());
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
		// if the gameObject is on a tile, start a new movement with new inputs.
		if (playerBody.TransformIsAlignedWithBody(transform.position)) {
			Vector3 directionVector = getInputAsVector ();

			// I like this movement pattern. If the game freezes for a few frames, then start movement from 0;
			// If the game is running smoothly, add an imaginary buffer frame so the first movement frame is at T+1.
			// beginMovementTime = Time.deltaTime > 0.05 ? Time.time : Time.time - Time.deltaTime;

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
			RaycastHit2D hit;
			bool moveSuccessful = playerBody.Move (velocity, out hit);
			if (!moveSuccessful)
				velocity = Vector3.zero;

		}

		transform.position = Vector3.MoveTowards(transform.position, playerBody.GetPos(), Time.deltaTime * speed);
		// targetTile - velocity is the old tile we are interpolating away from. 
	}
}
