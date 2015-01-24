using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ShoppingListUI : MonoBehaviour {

	private Text text;

	void Awake () {
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetList (List<Collectable> collectables) {
		text.text = string.Join("\n", collectables.Select(x => x.ToString()).ToArray());
	}
}
