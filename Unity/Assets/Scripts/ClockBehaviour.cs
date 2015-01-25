using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockBehaviour : MonoBehaviour {

	public float initialTime = 120.0f;
	public GameObject gameOverPanel;
	public GameObject youWinPanel;

	private Image clockImage;
	private float timeLeft;
	private PlayerBehaviour player;
	private bool hasWon = false;
	private float winFade = 0.0f;

	// Use this for initialization
	void Start () {
		timeLeft = initialTime;
		clockImage = GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

		gameOverPanel.SetActive(false);
		youWinPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (hasWon) {
			winFade += Time.deltaTime;
			youWinPanel.SetActive(true);
			youWinPanel.GetComponent<CanvasGroup>().alpha = Mathf.SmoothStep(0.0f, 1.0f, winFade / 0.25f);
		} else {
			timeLeft -= Time.deltaTime;
			clockImage.fillAmount = timeLeft / initialTime;

			if (timeLeft <= 0.0f) {
				player.isAlive = false;
				// Fade in "We're closed" UI.
				gameOverPanel.SetActive(true);
				gameOverPanel.GetComponent<CanvasGroup>().alpha = Mathf.SmoothStep(0.0f, 1.0f, -timeLeft / 0.25f);
			}
		}
	}

	public void Retry () {
		Debug.Log("Should retry.");
		Application.LoadLevel("Menu");
	}

	public void SetWin () {
		hasWon = true;
	}
}
