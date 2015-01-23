using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

	public float moveSpeed = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var camPos = Camera.main.transform.position;
		var camToMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)) - camPos;
		var t = - camPos.y / camToMouse.y;
		var planePos = camPos + t * camToMouse;
		var thisToMouse = planePos.ProjectY() - transform.position.ProjectY();
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, thisToMouse);

		if (Input.GetKey(KeyCode.W))
		{
			transform.position += (transform.rotation * Vector3.forward) * Time.deltaTime * moveSpeed;
		}
	}
}

static class VecUtils
{
	public static Vector3 ProjectY(this Vector3 vec)
	{
		return new Vector3(vec.x, 0.0f, vec.z);
	}
}
