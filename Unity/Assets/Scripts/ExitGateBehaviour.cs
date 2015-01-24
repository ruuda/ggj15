using UnityEngine;
using System.Collections;

public class ExitGateBehaviour : MonoBehaviour {

	private bool isFollowinig = true;
	private bool listDone = false;

	// Use this for initialization
	void Start () {
		UpdateCollider();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetFollowing (bool following) {
		isFollowinig = following;
		UpdateCollider();
	}

	public void SetDone (bool done) {
		listDone = done;
		UpdateCollider();
	}

	private void UpdateCollider () {
		var collider = GetComponent<BoxCollider>();
		if (isFollowinig && listDone) {
			collider.isTrigger = true; // May pass trough, and might as well detect when that happens.
			GetComponent<BoxCollider>().enabled = false;
			Debug.Log("The gate has been opened.");
		} else {
			collider.isTrigger = false; // YOU SHALL NOT PASS
			GetComponent<BoxCollider>().enabled = true;
			Debug.Log("The gate has been closed.");
		}
	}
}
