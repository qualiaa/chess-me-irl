using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CircleCollider2D))]
public class ForceField : MonoBehaviour {
	//CircleCollider2D collider_;

	void Start () {
		//collider_ = gameObject.AddComponent<CircleCollider2D> ();
		//collider_.radius = radius;
		//collider_.isTrigger = true;
	}

	void OnTriggerStay2D(Collider2D other) {
		Piece otherPiece = other.gameObject.GetComponent ("Piece") as Piece;
		PieceColor myColor = GetComponentInParent<Piece>().pieceColor;

		if (otherPiece != null && otherPiece.pieceColor != myColor)
		{
			var delta = other.transform.position - GetComponent<CircleCollider2D> ().transform.position;//collider_.transform.position;
			float magnitude = 50 / (1 + delta.magnitude);
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce(delta.normalized * magnitude);
		}
	}
}