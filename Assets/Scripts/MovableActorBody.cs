using UnityEngine;
using System.Collections;

public class MovableActorBody : MonoBehaviour {

	public float frameSpeed = 32f;

	private float inverseFrameSpeed;
	private Vector2 input;
	private Vector2 orientation;
	private Vector2 velocity;
	private Vector2 targetPos;

	void Start () {
		inverseFrameSpeed = 1 / frameSpeed; // 1 tile every 32 frames
		input = Vector2.zero; // This may change
		orientation = Vector2.down;
		velocity = Vector2.zero;
		targetPos = transform.position;
	}

	public void SetInput (Vector2 newInput){
		input = newInput;
	}

	bool BodyIsAlignedWithTarget (Vector3 transformPos)
	{
		if ( Mathf.Abs(transformPos.x - targetPos.x) < float.Epsilon) {
			if ( Mathf.Abs(transformPos.y - targetPos.y) < float.Epsilon) {
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		// if the gameObject is on a tile, start a new movement with new inputs.
		if (BodyIsAlignedWithTarget(transform.position)) {
			if (velocity == Vector2.zero) {
				if (Vector2.Dot (input, orientation) > 0) {
					// The input direction is the same as the orientation: move
					StartMoving ();
				} else if (input != Vector2.zero) {
					// The input direction is different: rotate
					RotateBodyWithAnimation ();
				}
				// Otherwise input was zero, ignore
			} else {
				if (input == Vector2.zero)
					velocity = Vector2.zero;
				else
					StartMoving ();
			}
		}

		// optionally check BodyIsAlignedWithTarget, not sure about performance
		transform.position = Vector2.MoveTowards(transform.position, targetPos, inverseFrameSpeed);
	}

	bool StartMoving () {
		if (Vector2.Dot (velocity, input) <= 0) { 
			// the input direction(s) is definitely different from the current velocity (which may be zero). Change velocity.
			orientation = GetOrientationFromInput ();
			velocity = orientation;
			// some kind of sprite trigger?
		}
		// otherwise there is a component in the direction of travel, which is dominant.

		bool moveSuccessful;
		Tile targetTile = LocalMap.GetTile ((Vector2)transform.position + velocity);
		if (targetTile.IsOutOfBounds()) {
			// We bonked. TODO: set animation trigger
			velocity = Vector2.zero; // don't walk off the map
			moveSuccessful = false;
		} else {
			moveSuccessful = targetTile.MoveIntoTileFrom (transform.position);
			if (!moveSuccessful) {
				// We bonked. TODO: set animation trigger
				velocity = Vector2.zero;
			} else {
				// we moved into target, now move out of current
				Tile currentTile = LocalMap.GetTile(transform.position);
				if (!currentTile.IsOutOfBounds())
					currentTile.MoveOut ();
			}
		}
		targetPos += velocity;
		return moveSuccessful;
	}

	Vector2 GetOrientationFromInput() {
		if (input.y > 0)
			return Vector2.up;
		else if (input.x < 0)
			return Vector2.left;
		else if (input.y < 0)
			return Vector2.down;
		else if (input.x > 0)
			return Vector2.right;
		return orientation; // if input is (0, 0) stay the same orientation
	}

	bool RotateBodyWithAnimation () {
		// TODO: actually implement this
		Vector2 newOrientation = GetOrientationFromInput ();
		if (orientation == newOrientation)
			return false;
		orientation = newOrientation;
		return true;
	}
}
