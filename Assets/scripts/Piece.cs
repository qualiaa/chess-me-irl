using UnityEngine;
using System.Collections;

public enum PieceColor { Black, White };

[RequireComponent (typeof (Renderer))]
public class Piece : MonoBehaviour {


	public PieceColor pieceColor = PieceColor.White;

	// Use this for initialization
	void Start () {
		if (pieceColor == PieceColor.Black)
		{
			Renderer r = GetComponent<Renderer> ();
			r.material.color = Color.black;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
