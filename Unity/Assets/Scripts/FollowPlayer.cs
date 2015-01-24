using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour {
	
	public float distance = 2f;
	public float angle = 10f;
	public float rotationSpeed = 10.0f;
	
	private PlayerBehaviour player;

	private Quaternion[] bufferedRotations;

	void Awake () {
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		this.camera.fieldOfView = 50.0f;
		this.bufferedRotations = new Quaternion[4];
	}

	void Start () {
		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		this.transform.position = this.player.waistPosition + this.player.transform.TransformDirection(follow);
		this.transform.LookAt(player.torsoPosition);
	}
	
	void LateUpdate () {
		bufferedRotations[0] = player.transform.rotation;
		for (int i = 1; i < bufferedRotations.Length; i++) {
			bufferedRotations[i] = Quaternion.Slerp(bufferedRotations[i], bufferedRotations[i - 1], Time.deltaTime * rotationSpeed);
		}
		
		var pos = player.transform.position;
		var rot = bufferedRotations[bufferedRotations.Length - 1];

		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		this.transform.position = pos + new Vector3(0.0f, 0.5f, 0.0f) + rot * follow;
		this.transform.LookAt((player.torsoPosition + player.waistPosition) / 2.0f);
	}
}
