using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockBehaviour : MonoBehaviour {

	public float initialTime = 120.0f;
	public GameObject gameOverPanel;

	private Image clockImage;
	private float timeLeft;
	private PlayerBehaviour player;

	// Use this for initialization
	void Start () {
		timeLeft = initialTime;
		clockImage = GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

		gameOverPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		clockImage.fillAmount = timeLeft / initialTime;

		if (timeLeft <= 0.0f) {
			player.isAlive = false;

			// Fade in "We're closed" UI.
			gameOverPanel.SetActive(true);
			gameOverPanel.GetComponent<CanvasGroup>().alpha = Mathf.SmoothStep(0.0f, 1.0f, -timeLeft / 0.2f);
		}
	}

	public void Retry () {
		Debug.Log("Should retry.");
	}
}
