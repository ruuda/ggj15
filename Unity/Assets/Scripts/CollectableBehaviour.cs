using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CollectableBehaviour : MonoBehaviour {

	public Collectable kind;
	public PointerBehaviour pointer;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			var shoppingList = other.gameObject.GetComponent<ShoppingListBehaviour>();
			shoppingList.Collect(kind, transform.position);

			if (pointer != null) pointer.Collect();
		}
	}
}
