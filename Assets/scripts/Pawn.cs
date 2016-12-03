using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (Piece))]
public class Pawn : MonoBehaviour {


	private Vector3 lastMousePos_;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {}

	void OnMouseDrag() {
		var newCameraPos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		var delta = newCameraPos - lastMousePos_;
		transform.Translate (delta);
		lastMousePos_ =  newCameraPos;
	}

	void OnMouseDown() {
		var r = GetComponent<Renderer> ();
		r.material.color = Color.cyan;
		lastMousePos_ = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
	}
	
	void OnMouseUp()
	{
		var r = GetComponent<Renderer> ();
		var p = GetComponent<Piece> ();

		r.material.color = p.pieceColor == PieceColor.White ? Color.white : Color.black;
	}
}
