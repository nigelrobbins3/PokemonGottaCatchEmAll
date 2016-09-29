using UnityEngine;
using System.Collections;

public class MovableActorBody2 {

	public struct AbsPos {
		public int x;
		public int y;
	}
	private AbsPos pos;

	public CircleCollider2D bodyCollider;

	public MovableActorBody2 (Vector3 position, CircleCollider2D collider)
	{
		pos.x = (int)position.x;
		pos.y = (int)position.y;
		bodyCollider = collider;
		bodyCollider.transform.position = GetPos ();
	}

	public Vector3 GetPos() {
		return new Vector3((float)pos.x, (float)pos.y, 0f);
	}

	public bool TransformIsAlignedWithBody (Vector3 transformPos)
	{
		if ( Mathf.Abs(transformPos.x - (float)pos.x) < float.Epsilon) {
			if ( Mathf.Abs(transformPos.y - (float)pos.y) < float.Epsilon) {
				return true;
			}
		}
		return false;
	}

	public bool Move(Vector3 velocity, out RaycastHit2D hit)
	{
		bodyCollider.enabled = false; // ignore self for movement collisions
		hit = Physics2D.Raycast(GetPos(), velocity, 1); // this has a depth property which could be used for bridges???
		bodyCollider.enabled = true;

		if (hit.collider == null) {
			pos.x += (int)velocity.x;
			pos.y += (int)velocity.y;
			bodyCollider.transform.position = GetPos ();
			return true;
		}
		return false;
	}
}
