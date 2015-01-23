using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

	public float moveSpeed = 5.0f;

	private Queue<Movement> movements = new Queue<Movement>();
	private float movementT;

	private Vector3 waistPosition { get { return this.transform.position + new Vector3(0f, 0.5f, 0f); } }


	void Start () {
	
	}

	void Update () {
		if (movements.Count == 0) {
			HandleInput();
		} else {
			HandleMove();
		}
	}

	private void HandleInput () {
		var left = transform.TransformDirection(Vector3.left);
		var right = transform.TransformDirection(Vector3.right);
		var forward = transform.forward;
		
		var canLeft = !Physics.Raycast(waistPosition, left, 0.6f);
		var canRight = !Physics.Raycast(waistPosition, right, 0.6f);
		var canForward = !Physics.Raycast(waistPosition, forward, 0.6f);
		
		Debug.DrawRay(waistPosition, left, Color.red, 0f);
		Debug.DrawRay(waistPosition, right, Color.blue, 0f);
		Debug.DrawRay(waistPosition, forward, Color.cyan, 0f);

		Debug.Log (canForward.ToString());

		if (canForward && Input.GetKey(KeyCode.W))
		{
			var move = new Movement { kind = MovementKind.Move, fromPos = this.transform.position, toPos = this.transform.position + forward };
			this.movements.Enqueue(move);
		}
	}

	private void HandleMove () {

		var movement = this.movements.Peek();
		this.movementT += Time.deltaTime;
		var t = this.movementT;

		// This one is done now.
		if (t > 1.0f)
		{
			this.movementT = 0.0f;
			t = 1.0f;
			this.movements.Dequeue();
		}

		// Ease / smooph step.
		t = Mathf.SmoothStep(0.0f, 1.0f, t);

		if (movement.kind == MovementKind.Move)
		{
			this.transform.position = movement.fromPos * (1.0f - t) + movement.toPos * t;
		}
	}
}

internal enum MovementKind {
	Rotate,
	Move
}

internal struct Movement {
	public MovementKind kind;
	public Vector3 fromPos;
	public Vector3 toPos;
	public Quaternion fromRot;
	public Quaternion toRot;
}
