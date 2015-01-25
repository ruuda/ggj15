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
		if (isFollowinig && listDone) {
			GetComponent<BoxCollider>().enabled = false; // May pass trough, and might as well detect when that happens.
			Debug.Log("The gate has been opened.");
		} else {
			GetComponent<BoxCollider>().enabled = true; // YOU SHALL NOT PASS
			Debug.Log("The gate has been closed.");
		}
	}
}
