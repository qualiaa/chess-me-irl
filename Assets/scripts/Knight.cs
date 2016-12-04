using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Knight : MonoBehaviour {

	Rigidbody2D body_;
	Vector3 lastMousePos_;

	void Start () {
		body_ = GetComponent<Rigidbody2D> ();
	}
		
	void OnMouseDown() {
		lastMousePos_ = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	void OnMouseUp() {
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		var delta = mousePos - lastMousePos_;

		body_.AddForce (new Vector2(-delta.x, -delta.y) * 100);
	}
}
