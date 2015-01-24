using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

	public float moveTimeRegular = 0.3f;
	public float moveTimeCrying = 0.6f;
	public float turnTime = 0.1f;
	
	private Queue<Movement> movements = new Queue<Movement>();
	private float movementT;
	private Vector3 waistPosition { get { return this.transform.position + new Vector3(0f, 0.5f, 0f); } }
	private ChildBehaviour child;

	void Awake () {
		child = GameObject.FindGameObjectWithTag("Child").GetComponent<ChildBehaviour>();
	}

	void Start () {

	}

	void Update () {
		if (movements.Count == 0) {
			HandleInput();
		} else {
			HandleMove();
		}
	}

	private Movement MakeMovement (Vector3 offset) {
		return new Movement {
			kind = MovementKind.Move,
			duration = child.isCrying ? moveTimeCrying : moveTimeRegular,
			fromPos = this.transform.position,
			toPos = this.transform.position + offset
		};
	}

	private Movement MakeRotation (float angle) {
		return new Movement {
			kind = MovementKind.Rotate,
			duration = turnTime,
			fromRot = this.transform.rotation,
			toRot = Quaternion.AngleAxis(angle, Vector3.up) * this.transform.rotation
		};
	}

	private void HandleInput () {
		var left = transform.TransformDirection(Vector3.left);
		var right = transform.TransformDirection(Vector3.right);
		var forward = transform.forward;
		
		var canLeft = !Physics.Raycast(waistPosition, left, 1.4f);
		var canRight = !Physics.Raycast(waistPosition, right, 1.4f);
		var canForward = !Physics.Raycast(waistPosition, forward, 1.4f);
		
		Debug.DrawRay(waistPosition, left * 1.4f, Color.red, 0f);
		Debug.DrawRay(waistPosition, right * 1.4f, Color.blue, 0f);
		Debug.DrawRay(waistPosition, forward * 1.4f, Color.cyan, 0f);

		if (canForward && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) {
			this.movements.Enqueue(MakeMovement(forward));
		}

		if (canLeft && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
			this.movements.Enqueue(MakeRotation(-90.0f));
			this.movements.Enqueue(MakeMovement(left));
		}

		if (canRight && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) {
			this.movements.Enqueue(MakeRotation(90.0f));
			this.movements.Enqueue(MakeMovement(right));
		}
	}

	private void HandleMove () {
		var movement = this.movements.Peek();
		this.movementT += Time.deltaTime ;
		var t = this.movementT / movement.duration;

		// This one is done now.
		if (t > 1.0f) {
			this.movementT = 0.0f;
			t = 1.0f;
			this.movements.Dequeue();
			MoveCompleted(movement);
		}

		// Ease / smooph step.
		t = Mathf.SmoothStep(0.0f, 1.0f, t);

		if (movement.kind == MovementKind.Move) {
			this.transform.position = movement.fromPos * (1.0f - t) + movement.toPos * t;
		}

		if (movement.kind == MovementKind.Rotate) {
			this.transform.rotation = Quaternion.Lerp(movement.fromRot, movement.toRot, t);
		}
	}

	private void MoveCompleted (Movement movement) {
		// Walking dicomforts the child.
		if (movement.kind == MovementKind.Move) {
			child.DecrementComfort();
		}
	}
}

internal enum MovementKind {
	Rotate,
	Move
}

internal struct Movement {
	public MovementKind kind;
	public float duration;
	public Vector3 fromPos;
	public Vector3 toPos;
	public Quaternion fromRot;
	public Quaternion toRot;
}
