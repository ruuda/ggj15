using UnityEngine;
using System.Collections;

public class PointerBehaviour : MonoBehaviour {

	private float angle = 0.0f;
	private Vector3 initialPosition;

	public float amplitude = 0.1f;
	public float rotationSpeed = 40.0f;
	public float hoverSpeed = 2.5f;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		angle += Time.deltaTime * hoverSpeed;

		this.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
		this.transform.position = initialPosition + new Vector3(0.0f, Mathf.Cos(angle) * amplitude);

		// TODO: Adjust height based on distance to player, with a maximum.
		// (Move down when a goal is near).
	}
}
