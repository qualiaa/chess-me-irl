using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Piece))]
[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (SpriteRenderer))]
public class King : MonoBehaviour {

	public float safeMass;
	public float dangerMass;
	PieceColor myColor_;
	Rigidbody2D body_;
	SpriteRenderer sprite_;

	void Start () {
		myColor_ = GetComponent<Piece> ().pieceColor;
		body_ = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		int i = Piece.GetTileIndex (transform);
		if (i >= 0) {
			Square square = GameObject.Find ("Board").transform.GetChild (i).GetComponent<Square>();

			PieceColor otherColor = (myColor_ == PieceColor.Black ? PieceColor.White : PieceColor.Black);

			if (square.threatenedBy [otherColor]) {
				body_.mass = dangerMass;
				square.SetColor (Color.red);
			} else {
				body_.mass = safeMass;
			}
		}
	}
}
