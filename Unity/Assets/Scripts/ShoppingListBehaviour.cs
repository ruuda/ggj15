﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ShoppingListBehaviour : MonoBehaviour {

	public int listLength = 5;
	public float listUIX = 0.7f;
	public float listUIY = 0.4f;
	public float listUIWidth = 0.25f;
	public ParticleController particleController;
	public GameObject pointerPrefab;
	public ExitGateBehaviour exitGate;

	public Sprite listTopSprite;
	public Sprite bananasSprite;
	public Sprite bananasCheckSprite;
	public Sprite strawberriesSprite;
	public Sprite strawberriesCheckSprite;
	public Sprite crispsSprite;
	public Sprite crispsCheckSprite;
	public Sprite milkSprite;
	public Sprite milkCheckSprite;
	public Sprite potatoesSprite;
	public Sprite potatoesCheckSprite;
	public Sprite kiwisSprite;
	public Sprite kiwisCheckSprite;
	public Sprite lemonsSprite;
	public Sprite lemonsCheckSprite;
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
		case Collectable.Strawberries: return strawberriesSprite;
		case Collectable.Crisps: return crispsSprite;
		case Collectable.Kiwis: return kiwisSprite;
		case Collectable.Lemons: return lemonsSprite;
		case Collectable.Milk: return milkSprite;
		case Collectable.Potatoes: return potatoesSprite;
		}
		throw new System.ArgumentException("collectable");
	}

	private Sprite CollectableToCheckSprite (Collectable collectable) {
		switch (collectable) {
		case Collectable.Bananas: return bananasCheckSprite;
		case Collectable.Strawberries: return strawberriesCheckSprite;
		case Collectable.Crisps: return crispsCheckSprite;
		case Collectable.Kiwis: return kiwisCheckSprite;
		case Collectable.Lemons: return lemonsCheckSprite;
		case Collectable.Milk: return milkCheckSprite;
		case Collectable.Potatoes: return potatoesCheckSprite;
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

	public void Collect (Collectable collectable, Vector3 at) {
		var index = this.collectables.IndexOf(collectable);
		if (this.collectables.Remove(collectable)) {
			Debug.Log(string.Format ("{0} collected", collectable));
			// TODO: Special effects, whatnot.

			// The index in the ui Image list is +1 because of the list top, check it now.
			uiImages[index + 1].sprite = CollectableToCheckSprite(collectable);

			// Spawn particles for visual feedback.
			this.particleController.Burst(at);

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
