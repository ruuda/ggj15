using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public float smoothingSpeed = 12.0f;
	public int smoothingRank = 15;
	public int aimAtRank = 10;
	public float distance = 3f;
	public float angle = 25.0f;
	
	private PlayerBehaviour player;

	private Vector3[] positionBuffer;
	private Vector3[] lookAtBuffer;

	void Awake () {
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
		this.positionBuffer = new Vector3[smoothingRank];
		this.lookAtBuffer = new Vector3[aimAtRank];
	}

	void Start () {
		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		this.transform.position = this.player.waistPosition + this.player.transform.TransformDirection(follow);
		this.transform.LookAt(player.waistPosition);

		for (int i = 0; i < positionBuffer.Length; i++) {
			positionBuffer[i] = this.transform.position;
		}
		for (int i = 0; i < lookAtBuffer.Length; i++) {
			lookAtBuffer[i] = player.waistPosition;
		}
	}
	
	void Update () {
		var follow = new Vector3(0.0f, Mathf.Sin(angle / 180.0f * Mathf.PI), -Mathf.Cos(angle / 180.0f * Mathf.PI)) * distance;
		var desired = player.waistPosition + this.player.transform.TransformDirection(follow);
		positionBuffer[0] = desired;
		lookAtBuffer[0] = player.waistPosition;

		for (int i = 1; i < positionBuffer.Length; i++) {
			positionBuffer[i] += (positionBuffer[i - 1] - positionBuffer[i]) * smoothingSpeed * Time.deltaTime;
		}
		for (int i = 1; i < lookAtBuffer.Length; i++) {
			lookAtBuffer[i] += (lookAtBuffer[i - 1] - lookAtBuffer[i]) * smoothingSpeed * Time.deltaTime;
		}

		this.transform.position = positionBuffer[positionBuffer.Length - 1];
		this.transform.LookAt(lookAtBuffer[lookAtBuffer.Length - 1]);
	}
}
