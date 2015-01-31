using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ShoppingListBehaviour : MonoBehaviour {

	public int listLength = 5;
	public float listUIX = 0.7f;
	public float listUIY = 0.05f;
	public float listUIWidth = 0.25f;
	public ParticleController particleController;
	public GameObject pointerPrefab;
	public GameObject candyPointerPrefab;
	public ExitGateBehaviour exitGate;
	public AudioController audioController;

	public Sprite listApplesSprite;
	public Sprite listBananasSprite;
	public Sprite listCheeseSprite;
	public Sprite listCucumbersSprite;
	public Sprite listEggsSprite;
	public Sprite listGrapesSprite;
	public Sprite listHamSprite;
	public Sprite listMilkSprite;
	public Sprite listPeppersSprite;
	public Sprite listSausagesSprite;

	public Sprite listApplesCheckSprite;
	public Sprite listBananasCheckSprite;
	public Sprite listCheeseCheckSprite;
	public Sprite listCucumbersCheckSprite;
	public Sprite listEggsCheckSprite;
	public Sprite listGrapesCheckSprite;
	public Sprite listHamCheckSprite;
	public Sprite listMilkCheckSprite;
	public Sprite listPeppersCheckSprite;
	public Sprite listSausagesCheckSprite;

	public Sprite listTopSprite;
	public Sprite listBottomSprite;

	private List<Collectable> collectables = new List<Collectable>();
	private List<Collectable> uiList;

	private Canvas uiCanvas;
	private List<Image> uiImages = new List<Image>();

	void Awake () {
		// Get a random subset of the available items of the desired length.
		var available = ((Collectable[])System.Enum.GetValues(typeof(Collectable))).ToList ();
		available.Shuffle();
		for (int i = 0; i < listLength; i++) {
			collectables.Add(available[i]);
		}
		uiList = new List<Collectable>(collectables);
	}

	private Sprite CollectableToSprite (Collectable collectable) {
		switch (collectable) {
		case Collectable.Apples: return listApplesSprite;
		case Collectable.Bananas: return listBananasSprite;
		case Collectable.Cheese: return listCheeseSprite;
		case Collectable.Cucumbers: return listCucumbersSprite;
		case Collectable.Eggs: return listEggsSprite;
		case Collectable.Grapes: return listGrapesSprite;
		case Collectable.Ham: return listHamSprite;
		case Collectable.Milk: return listMilkSprite;
		case Collectable.Peppers: return listPeppersSprite;
		case Collectable.Sausages: return listSausagesSprite;
		}
		throw new System.ArgumentException("collectable");
	}

	private Sprite CollectableToCheckSprite (Collectable collectable) {
		switch (collectable) {
		case Collectable.Apples: return listApplesCheckSprite;
		case Collectable.Bananas: return listBananasCheckSprite;
		case Collectable.Cheese: return listCheeseCheckSprite;
		case Collectable.Cucumbers: return listCucumbersCheckSprite;
		case Collectable.Eggs: return listEggsCheckSprite;
		case Collectable.Grapes: return listGrapesCheckSprite;
		case Collectable.Ham: return listHamCheckSprite;
		case Collectable.Milk: return listMilkCheckSprite;
		case Collectable.Peppers: return listPeppersCheckSprite;
		case Collectable.Sausages: return listSausagesCheckSprite;
		}
		throw new System.ArgumentException("collectable");
	}

	void CreateUI () {
		var canvasObject = new GameObject("Canvas");
		canvasObject.layer = LayerMask.NameToLayer("UI");
		uiCanvas = canvasObject.AddComponent<Canvas>();
		uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		uiCanvas.pixelPerfect = true;

		var sprites = new List<Sprite>();
		sprites.Add(listTopSprite);
		sprites.AddRange(collectables.Select(c => CollectableToSprite(c)));
		sprites.Add(listBottomSprite);
		sprites.Reverse();

		var origin = new Vector2(listUIX, listUIY);
		foreach (var sprite in sprites) {
			var spriteObject = new GameObject("Image" + sprite.name);
			spriteObject.transform.parent = canvasObject.transform;
			var image = spriteObject.AddComponent<Image>();
			image.sprite = sprite;
			var uiHeight = sprite.bounds.size.y / sprite.bounds.size.x * listUIWidth * Camera.main.aspect;
			image.rectTransform.anchorMin = origin;
			image.rectTransform.anchorMax = origin + new Vector2(listUIWidth, uiHeight);
			image.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
			image.rectTransform.sizeDelta = new Vector2(1.0f, 1.0f);
			origin += new Vector2(0.0f, uiHeight);
			uiImages.Add(image);
		}
		uiImages.Reverse();
	}

	// Use this for initialization
	void Start () {
		CreateUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Collect (Collectable collectable, Vector3 at) {
		var index = this.uiList.IndexOf(collectable);
		if (this.collectables.Remove(collectable)) {
			Debug.Log(string.Format ("{0} collected", collectable));

			// The index in the ui Image list is +1 because of the list top, check it now.
			uiImages[index + 1].sprite = CollectableToCheckSprite(collectable);

			// Spawn particles for visual feedback.
			this.particleController.Burst(at);

			// Play collect sound.
			this.audioController.CollectAt(at);

			// If this was the last one, the gate may be opened.
			if (this.collectables.Count == 0) {
				exitGate.SetDone(true);
			}
		} else {
			Debug.Log(string.Format("{0} not collected, not on list", collectable));
		}
	}

	public PointerBehaviour MaybeSpawnPrefabAt (Collectable collectable, Vector3 at) {
		if (this.collectables.Contains(collectable)) {
			return ((GameObject)GameObject.Instantiate(pointerPrefab, at + Vector3.up, Quaternion.identity)).GetComponent<PointerBehaviour>();
		}
		return null;
	}

	public void SpawnCandyPrefabAt (Vector3 at) {
		var pointer = GameObject.Instantiate(candyPointerPrefab, at + Vector3.up, Quaternion.identity) as GameObject;
		pointer.transform.Rotate(Vector3.forward, -90.0f);
	}
}

public enum Collectable {
	Apples,
	Bananas,
	Cheese,
	Cucumbers,
	Eggs,
	Grapes,
	Ham,
	Milk,
	Peppers,
	Sausages
}

static class ListExtensions {
	public static void Shuffle<T> (this IList<T> list) {  
		var rng = new System.Random();
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
