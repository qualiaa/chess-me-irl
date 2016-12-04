using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpringJoint2D))]
public class Rook : MonoBehaviour {

	SpringJoint2D spring_;
	Vector2 offset_;

	void Start () {
		spring_ = GetComponent<SpringJoint2D> ();
		spring_.autoConfigureConnectedAnchor = false;
	}

	void OnMouseDrag() {
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var mouse2D = new Vector2 (mousePos.x, mousePos.y);
		spring_.connectedAnchor = mouse2D - offset_;	
	}

	void OnMouseDown() {
		var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var cam2D = new Vector2 (cameraPos.x, cameraPos.y);
		offset_ = cam2D - spring_.connectedAnchor;	
	}
}
