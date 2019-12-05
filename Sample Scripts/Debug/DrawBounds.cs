using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBounds : MonoBehaviour {

	Bounds bounds;

	void Start() {
		if (GetComponent<Renderer> ()) {
			bounds = GetComponent<Renderer> ().bounds;
		} else {
			bounds = GetComponent<Collider> ().bounds;
		}
//		Debug.LogError (transform.name + " center: " + bounds.center + ", size: " + bounds.size);
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (bounds.center, bounds.size);
	}
}
