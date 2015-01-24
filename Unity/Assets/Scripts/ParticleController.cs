using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

	ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Burst (Vector3 at) {
		Debug.Log("Spawning particles");
		ps.transform.position = at;
		ps.Emit(300);
	}
}
