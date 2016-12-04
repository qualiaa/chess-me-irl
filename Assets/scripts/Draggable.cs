using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TargetJoint2D))]
public class Draggable : MonoBehaviour {

	private TargetJoint2D target_;

	void Start () {
		target_ = GetComponent<TargetJoint2D> ();
	}

	void OnMouseDrag() {
		target_.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDown() {
		target_.enabled = true;
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target_.anchor = mousePos - transform.position;
		target_.target = mousePos;
	}
	
	void OnMouseUp()
	{
		target_.enabled = false;
	}
}
