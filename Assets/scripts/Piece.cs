using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PieceColor { Black, White };
public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn};

[RequireComponent (typeof (Renderer))]
public class Piece : MonoBehaviour {

	public PieceColor pieceColor = PieceColor.White;
	public PieceType pieceType = PieceType.Pawn;

	void Start () {
		if (pieceColor == PieceColor.Black)
		{
			Renderer r = GetComponent<Renderer> ();
			r.material.color = Color.black;
		}
	}
}
