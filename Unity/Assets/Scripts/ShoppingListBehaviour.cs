using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShoppingListBehaviour : MonoBehaviour {

	public int listLength = 5;

	private List<Collectable> collectables = new List<Collectable>();
	private ShoppingListUI shoppingListUI;

	void Awake () {
		// Get a random subset of the available items of the desired length.
		var available = ((Collectable[])System.Enum.GetValues(typeof(Collectable))).ToList ();
		available.Shuffle();
		for (int i = 0; i < listLength; i++) {
			collectables.Add(available[i]);
		}

		shoppingListUI = GameObject.FindGameObjectWithTag("ShoppingListText").GetComponent<ShoppingListUI>();
	}

	// Use this for initialization
	void Start () {
		shoppingListUI.SetList(this.collectables);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Collect(Collectable collectable) {
		if (this.collectables.Remove(collectable)) {
			Debug.Log(string.Format ("{0} collected", collectable));
			// TODO: Special effects, whatnot.
			shoppingListUI.SetList(this.collectables);
		} else {
			Debug.Log(string.Format("{0} not collected, not on list", collectable));
		}
	}
}

public enum Collectable {
	Banana,
	Strawberry,
	Crisps,
	Milk,
	Potato,
	Kiwi,
	Lemon
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
