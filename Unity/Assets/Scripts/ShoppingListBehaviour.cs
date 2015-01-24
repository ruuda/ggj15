using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ShoppingListBehaviour : MonoBehaviour {

	public int listLength = 5;
	public float listUIX = 0.7f;
	public float listUIY = 0.4f;
	public float listUIWidth = 0.25f;

	public Sprite listTopSprite;
	public Sprite bananasSprite;
	public Sprite strawberriesStprite;
	public Sprite crispsSprite;
	public Sprite milkSprite;
	public Sprite potatoesSprite;
	public Sprite kiwisSprite;
	public Sprite lemonsSprite;
	public Sprite listBottomSprite;

	private List<Collectable> collectables = new List<Collectable>();

	private Canvas uiCanvas;
	private List<Image> uiImages = new List<Image>();

	void Awake () {
		// Get a random subset of the available items of the desired length.
		var available = ((Collectable[])System.Enum.GetValues(typeof(Collectable))).ToList ();
		available.Shuffle();
		for (int i = 0; i < listLength; i++) {
			collectables.Add(available[i]);
		}
	}

	private Sprite CollectableToSprite (Collectable collectable) {
		switch (collectable) {
		case Collectable.Bananas: return bananasSprite;
		case Collectable.Strawberries: return strawberriesStprite;
		case Collectable.Crisps: return crispsSprite;
		case Collectable.Kiwis: return kiwisSprite;
		case Collectable.Lemons: return lemonsSprite;
		case Collectable.Milk: return milkSprite;
		case Collectable.Potatoes: return potatoesSprite;
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

		var origin = new Vector2(listUIX, listUIY);
		foreach (var sprite in sprites) {
			var spriteObject = new GameObject("Image" + sprite.name);
			spriteObject.transform.parent = canvasObject.transform;
			var image = spriteObject.AddComponent<Image>();
			image.sprite = sprite;
			var uiHeight = sprite.bounds.size.y / sprite.bounds.size.x * listUIWidth * Camera.main.aspect;
			image.rectTransform.anchorMin = origin - new Vector2(0.0f, uiHeight);
			image.rectTransform.anchorMax = origin + new Vector2(listUIWidth, 0.0f);
			image.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
			image.rectTransform.sizeDelta = new Vector2(1.0f, 1.0f);
			origin -= new Vector2(0.0f, uiHeight);
			uiImages.Add (image);
		}		
	}

	// Use this for initialization
	void Start () {
		CreateUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Collect(Collectable collectable) {
		var index = this.collectables.IndexOf(collectable);
		if (this.collectables.Remove(collectable)) {
			Debug.Log(string.Format ("{0} collected", collectable));
			// TODO: Special effects, whatnot.

			// The index in the ui Image list is +1 because of the list top.
			uiImages[index + 1].color = Color.black; // TODO: swap image? overlay?
		} else {
			Debug.Log(string.Format("{0} not collected, not on list", collectable));
		}
	}
}

public enum Collectable {
	Bananas,
	Strawberries,
	Crisps,
	Milk,
	Potatoes,
	Kiwis,
	Lemons
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
