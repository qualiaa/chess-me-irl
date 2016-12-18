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

	public static int GetTileIndex(Transform t)
	{
		int x = (int)(t.position.x + 4f);
		int y = (int)(-t.position.y + 4f);

		if (x < 0 || y < 0 || x >= 8 || y >= 8) {
			return -1;
		}

		return x + y * 8;
	}
}
