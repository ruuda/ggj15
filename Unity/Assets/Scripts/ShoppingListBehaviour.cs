using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShoppingListBehaviour : MonoBehaviour {

	public int listLength = 5;

	private Queue<Collectable> collectables = new Queue<Collectable>();

	void Awake () {
		var available = ((Collectable[])System.Enum.GetValues(typeof(Collectable))).ToList ();
		available.Shuffle();
		for (int i = 0; i < listLength; i++) {
			collectables.Enqueue(available[i]);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
