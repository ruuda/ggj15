using UnityEngine;
using System.Collections;

public class PointerBehaviour : MonoBehaviour {

	private float angle = 0.0f;
	private Vector3 initialPosition;
	private float collectAngle = 0.0f;

	public float amplitude = 0.1f;
	public float rotationSpeed = 40.0f;
	public float hoverSpeed = 2.5f;

	bool isCollected = false;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		angle += Time.deltaTime * hoverSpeed;

		float sqrPlayer = (GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position).sqrMagnitude;
		float extraHeight = Mathf.Clamp01(sqrPlayer / 25.0f) * 1.5f;

		this.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
		this.transform.position = initialPosition + new Vector3(0.0f, Mathf.Cos(angle) * amplitude + extraHeight, 0.0f);

		if (isCollected) {
			collectAngle += Time.deltaTime * 3.0f;
			var hover = Mathf.Sin(collectAngle);
			var cancel = Mathf.Cos (collectAngle);
			this.transform.position = initialPosition + new Vector3(0.0f, Mathf.Cos(angle) * amplitude * cancel + hover * 5.0f, 0.0f);

			if (collectAngle > Mathf.PI * 0.5) Destroy(this);
		}

		// TODO: Adjust height based on distance to player, with a maximum.
		// (Move down when a goal is near).
	}

	public void Collect () {
		isCollected = true;
	}
}
