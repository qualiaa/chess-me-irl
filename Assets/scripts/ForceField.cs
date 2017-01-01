using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CircleCollider2D))]
public class ForceField : MonoBehaviour {
	SpriteRenderer sprite_;

	int n_enemies_ = 0;

	void Start () {
		sprite_ = GetComponent<SpriteRenderer> ();
		sprite_.material.SetVector("_StartTime", new Vector4(0, 0, 0));
	}

	bool isEnemy(Collider2D other) {
		Piece otherPiece = other.gameObject.GetComponent ("Piece") as Piece;
		PieceColor myColor = GetComponentInParent<Piece>().pieceColor;

		return (otherPiece != null && otherPiece.pieceColor != myColor);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (isEnemy(other))
		{
			var delta = other.transform.position - GetComponent<CircleCollider2D> ().transform.position;//collider_.transform.position;
			float magnitude = 50 / (1 + delta.magnitude);
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce(delta.normalized * magnitude);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isEnemy (other)) {
			if (n_enemies_ == 0) {
				sprite_.material.SetVector("_StartTime", new Vector4(Time.time, 0, 1));
			}
			++n_enemies_;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (isEnemy (other)) {
			--n_enemies_;
			if (n_enemies_ == 0) {
				var t = sprite_.material.GetVector ("_StartTime");
				t.y = Time.time;
				t.z = 0;
				sprite_.material.SetVector ("_StartTime", t);
			}
		}


	}

	void OnMouseClick() {}
	void OnMouseDown() {}
	void OnMouseDrag() {} 
}