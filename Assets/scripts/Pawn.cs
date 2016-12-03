using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TargetJoint2D))]
public class Pawn : MonoBehaviour {

	private TargetJoint2D target_;

	// Use this for initialization
	void Start () {
		target_ = GetComponent<TargetJoint2D> ();
	}
	
	// Update is called once per frame
	void Update () {}

	void OnMouseDrag() {
		target_.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseDown() {
		target_.enabled = true;
		var cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target_.anchor = cameraPos-transform.position;
		target_.target = cameraPos;
	}
	
	void OnMouseUp()
	{
		target_.enabled = false;
	}
}
