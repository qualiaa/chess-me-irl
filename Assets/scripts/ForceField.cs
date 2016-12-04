using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Piece))]
public class ForceField : MonoBehaviour {

	public float radius;

	//CircleCollider2D collider_;

	// Use this for initialization
	void Start () {
		//collider_ = gameObject.AddComponent<CircleCollider2D> ();
		//collider_.radius = radius;
		//collider_.isTrigger = true;
	}

	void OnTriggerStay2D(Collider2D other) {
		Piece otherPiece = other.gameObject.GetComponent ("Piece") as Piece;
		PieceColor myColor = GetComponent<Piece>().pieceColor;

		Debug.Log ("Trigger pulled!");

		if (otherPiece != null && otherPiece.pieceColor != myColor)
		{
			var delta = other.transform.position - GetComponent<CircleCollider2D> ().transform.position;//collider_.transform.position;
			float magnitude = 100 / (1 + delta.magnitude);
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce(delta.normalized * magnitude);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
