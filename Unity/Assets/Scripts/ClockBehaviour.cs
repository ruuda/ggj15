using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockBehaviour : MonoBehaviour {

	public float initialTime = 120.0f;

	private Image clockImage;
	private float timeLeft;

	// Use this for initialization
	void Start () {
		timeLeft = initialTime;
		clockImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		clockImage.fillAmount = timeLeft / initialTime;
		Debug.Log(string.Format("Fill is {0}", timeLeft / initialTime));
	}
}
