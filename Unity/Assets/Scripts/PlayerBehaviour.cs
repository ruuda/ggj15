using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

	public float moveTimeRegular = 0.3f;
	public float moveTimeCrying = 0.6f;
	public float turnTime = 0.1f;
	public Vector3 waistPosition { get { return this.transform.position + new Vector3(0f, 0.5f, 0f); } }
	public Vector3 torsoPosition { get { return this.transform.position + new Vector3(0f, 1.0f, 0f); } }
	public bool isAlive { get; set; }
	
	private Queue<Movement> movements = new Queue<Movement>();
	private float movementT;
	private ChildBehaviour child;

	public float movementTime { get { return movementT; } }

	void Awake () {
		child = GameObject.FindGameObjectWithTag("Child").GetComponent<ChildBehaviour>();
		isAlive = true;
	}

	void Start () {

	}

	void Update () {
		if (movements.Count == 0) {
			HandleInput();
			RestoreAnimation();
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
		if (!isAlive) return;

		var left = transform.TransformDirection(Vector3.left);
		var right = transform.TransformDirection(Vector3.right);
		var forward = transform.forward;
		
		var canLeft = !Physics.Raycast(waistPosition, left, 1.4f);
		var canRight = !Physics.Raycast(waistPosition, right, 1.4f);
		var canForward = !Physics.Raycast(waistPosition, forward, 1.4f);
		
		Debug.DrawRay(waistPosition, left * 1.4f, Color.red, 0f);
		Debug.DrawRay(waistPosition, right * 1.4f, Color.blue, 0f);
		Debug.DrawRay(waistPosition, forward * 1.4f, Color.cyan, 0f);

		var goLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		var goRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		var goForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

		// Prefer touch starting, that is a tap.
		bool touchDetected = false;
		foreach (var touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				float x = touch.position.x / Camera.main.pixelWidth;

				if (x < 0.35) goLeft = true;
				else if (x > 0.65) goRight = true;
				else goForward = true;
				touchDetected = true;
				break;
			}
		}

		// If there was no touch started, a stationary touch will do.
		if (!touchDetected) {
		foreach (var touch in Input.touches) {
				if (touch.phase == TouchPhase.Stationary) {
					float x = touch.position.x / Camera.main.pixelWidth;
					
					if (x < 0.35) goLeft = true;
					else if (x > 0.65) goRight = true;
					else goForward = true;
					break;
				}
			}
		}

		if (canLeft && goLeft) {
			this.movements.Enqueue(MakeRotation(-90.0f));
			this.movements.Enqueue(MakeMovement(left));
			return;
		}

		if (canRight && goRight) {
			this.movements.Enqueue(MakeRotation(90.0f));
			this.movements.Enqueue(MakeMovement(right));
			return;
		}

		if (canForward && goForward) {
			this.movements.Enqueue(MakeMovement(forward));
			return;
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

		if (movement.kind == MovementKind.Move) {
			this.transform.position = movement.fromPos * (1.0f - t) + movement.toPos * t;

			if (t < 1.0f) {
				animation.Play("Walk");
				child.animation.Play("Walk");
			}
			animation["Walk"].speed = animation["Walk"].clip.length / movement.duration;
			child.animation["Walk"].speed = animation["Walk"].clip.length / movement.duration;
		}

		if (movement.kind == MovementKind.Rotate) {
			this.transform.rotation = Quaternion.Lerp(movement.fromRot, movement.toRot, Mathf.SmoothStep(0.0f, 1.0f, t));
		}
	}

	private void RestoreAnimation () {
		// TODO: Should mix with Idle animation state.
		animation["Walk"].weight -= animation["Walk"].weight * Time.deltaTime;
	}
	
	private void MoveCompleted (Movement movement) {
		// Walking dicomforts the child.
		if (movement.kind == MovementKind.Move) {
			child.DecrementComfort();
		}
	}
}

public enum MovementKind {
	Rotate,
	Move
}

public struct Movement {
	public MovementKind kind;
	public float duration;
	public Vector3 fromPos;
	public Vector3 toPos;
	public Quaternion fromRot;
	public Quaternion toRot;
}
