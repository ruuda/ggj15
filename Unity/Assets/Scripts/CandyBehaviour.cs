using UnityEngine;
using System.Collections;

public class CandyBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Child") {
			other.gameObject.GetComponent<ChildBehaviour>().GiveCandy(this.transform.position);
		}
	}
}
