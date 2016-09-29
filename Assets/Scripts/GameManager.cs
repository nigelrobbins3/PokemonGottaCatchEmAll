using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public MovableActorBody player;
	public MovableActorBody trainer;

	void Awake () {
		// make singleton
		if (instance == null)
			instance = this;
		if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);

		LocalMap.LoadMap (Vector2.zero);
		LocalMap.RenderMap ();

		GameObject playerbody = Instantiate(
			Resources.Load("Prefabs/PlayerBody"),
			new Vector3(-5f,3f,0f),
			Quaternion.identity,
			this.transform
		) as GameObject;
		player = playerbody.GetComponent<MovableActorBody>();

		GameObject trainerbody = Instantiate(
			Resources.Load("Prefabs/DummyTrainer"),
			new Vector3(2f,3f,0f),
			Quaternion.identity,
			this.transform
		) as GameObject;
		trainer = trainerbody.GetComponent<MovableActorBody>();
		trainerbody.GetComponent<SpriteRenderer> ().color = Color.green;
	}

	Vector2 getInputAsVector(){
		Vector2 direction = Vector2.zero;
		if (Input.GetButton ("rightKB")) direction.x += 1;
		if (Input.GetButton ("leftKB")) direction.x -= 1;
		if (Input.GetButton ("upKB")) direction.y += 1;
		if (Input.GetButton ("downKB")) direction.y -= 1;
		return direction;
	}

	private Vector2 trainerInput = Vector2.down;
	void Update () {
		if (trainer.gameObject.transform.position.y < -3)
			trainerInput = Vector2.up;
		if (trainer.gameObject.transform.position.y > 3)
			trainerInput = Vector2.down;
		trainer.SetInput (trainerInput);
		player.SetInput (getInputAsVector ());
	}	
}
