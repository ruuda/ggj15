using UnityEngine;
using System.Collections;

public class WinTriggerBehaviour : MonoBehaviour {

	public ClockBehaviour clock;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log("The player may have won the game.");
			clock.SetWin();
		}
	}
}
