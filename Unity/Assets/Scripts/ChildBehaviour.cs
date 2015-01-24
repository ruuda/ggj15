using UnityEngine;
using System.Collections;

public class ChildBehaviour : MonoBehaviour {

	private bool isFollowing = true;
	private GameObject player;
	private Vector3 initialOffset;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag("Player");
		initialOffset = transform.position - player.transform.position;
		initialRotation = transform.rotation * Quaternion.Inverse(player.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isFollowing) {
			this.transform.position = player.transform.position + initialOffset;
			this.transform.rotation = player.transform.rotation * initialRotation;
		}
	}
}
