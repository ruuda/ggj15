using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public float smoothingSpeed = 1.0f;
	public float distance = 10.0f;
	public float angle = 45.0f;

	private GameObject player;

	void Awake () {
		this.player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start () {
		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		this.transform.position = this.player.transform.position + this.player.transform.TransformDirection(follow);
		this.transform.LookAt(player.transform.position);
	}
	
	void Update () {
		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		var desired = this.player.transform.position + this.player.transform.TransformDirection(follow);
		var delta = desired - this.transform.position;
		this.transform.position += delta * smoothingSpeed * Time.deltaTime;

		this.transform.LookAt(player.transform.position);
	}
}
