using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject initialPanel;
	public GameObject controlsPanel;

	public float transitionDuration = 0.2f;

	private CanvasGroup initialGroup;
	private CanvasGroup controlsGroup;

	private float t = 0.0f;
	private bool transitioning = false;

	// Use this for initialization
	void Start () {
		controlsGroup = controlsPanel.GetComponent<CanvasGroup>();
		controlsGroup.alpha = 0.0f;
		controlsPanel.SetActive(false);
		initialGroup = initialPanel.GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		if (transitioning) {
			t += Time.deltaTime / transitionDuration;
			if (t > 1.0f) {
				t = 1.0f;
				transitioning = false;
			}

			controlsGroup.alpha = Mathf.SmoothStep(0.0f, 1.0f, t);
			initialGroup.alpha = 1.0f - controlsGroup.alpha;
		}
	}

	public void InitialPlayClicked () {
		Debug.Log("Showing controls menu");
		controlsPanel.SetActive(true);
		transitioning = true;
	}

	public void ControlsPlayClicked () {
		Debug.Log("Starting game");
		Application.LoadLevel("MainSceneOld"); // TODO: -> Grocery
	}
}
