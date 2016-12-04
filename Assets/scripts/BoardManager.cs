using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	GameObject[] pieces_;
	Square[,] squares_;

	void Start () {
		pieces_= GameObject.FindGameObjectsWithTag("Piece");
		squares_ = new Square[8,8];

		for (int i = 0; i < 64; ++i)
		{
			squares_ [i % 8, i / 8] = transform.GetChild (i).gameObject.GetComponent<Square>();
		}
	}

	void SetColor(int x, int y, Color c)
	{
		if (x < 0 || y < 0 || x >= 8 || y >= 8) {
			return;
		}

		Image image = squares_[x,y].gameObject.GetComponentInParent<Image> ();
		image.color = c;
	}

	void bishopThreat(int x, int y, Color c)
	{
		for (int i = 1; i < 8; ++i) {
			SetColor (x - i, y - i, c);
			if (isOccupied(x-i,y-i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x - i, y + i, c);
			if (isOccupied(x-i,y+i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x + i, y - i, c);
			if (isOccupied(x+i,y-i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x + i, y + i, c);
			if (isOccupied(x+i,y+i)) break;
		}
	}

	void rookThreat(int x, int y, Color c)
	{
		for (int i = 1; i < 8; ++i) {
			SetColor (x - i, y, c);
			if (isOccupied(x - i, y)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x + i, y, c);
			if (isOccupied(x + i, y)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x, y - i, c);
			if (isOccupied(x, y - i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetColor (x, y + i, c);
			if (isOccupied(x, y + i)) break;
		}
	}

	void knightThreat(int x, int y, Color c)
	{
		SetColor (x - 2, y - 1, c);
		SetColor (x - 2, y + 1, c);
		SetColor (x + 2, y - 1, c);
		SetColor (x + 2, y + 1, c);
		SetColor (x - 1, y - 2, c);
		SetColor (x - 1, y + 2, c);
		SetColor (x + 1, y - 2, c);
		SetColor (x + 1, y + 2, c);
	}

	void kingThreat(int x, int y, Color c)
	{
		SetColor (x + 1, y - 1, c);
		SetColor (x + 1, y + 1, c);
		SetColor (x - 1, y - 1, c);
		SetColor (x - 1, y + 1, c);
		SetColor (x    , y - 1, c);
		SetColor (x - 1, y    , c);
		SetColor (x    , y + 1, c);
		SetColor (x + 1, y    , c);
	}

	void pawnThreat(int x, int y, Color c)
	{
		int y1 = c == Color.black ? y+1 : y - 1;
		SetColor (x - 1, y1, c);
		SetColor (x + 1, y1, c);
	}

	void setOccupied(int x, int y)
	{
		if (x < 0 || y < 0 || x >= 8 || y >= 8)
		{
			return;
		}

		squares_ [x, y].occupied = true;
	}

	bool isOccupied(int x, int y)
	{
		if (x < 0 || y < 0 || x >= 8 || y >= 8)
		{
			return false;
		}

		return squares_ [x, y].occupied;
	}

	// Update is called once per frame
	void Update () {
		BroadcastMessage ("Clear");
		/*
		foreach (var square in squares_)
		{
			square.Clear();
		}
		*/

		foreach (var pieceGO in pieces_)
		{
			Piece piece = pieceGO.GetComponent<Piece> ();
			
			var fBoardPos = pieceGO.transform.position;
			fBoardPos.x += 3.5f;
			fBoardPos.y *= -1f;
			fBoardPos.y += 3.5f;
			int x = (int)Mathf.Round(fBoardPos.x);
			int y = (int)Mathf.Round(fBoardPos.y);
			setOccupied (x, y);//SetColor (x, y, Color.blue);

			Color pCol = piece.pieceColor == PieceColor.White ? Color.white : Color.black;
			pCol.a = 0.3f;
			switch (piece.pieceType) {
			case PieceType.Bishop:
				bishopThreat (x, y, pCol);
				break;
			case PieceType.Rook:
				rookThreat (x, y, pCol);
				break;
			case PieceType.Queen:
				bishopThreat (x, y, pCol);
				rookThreat (x, y, pCol);
				break;
			case PieceType.Knight:
				knightThreat (x, y, pCol);
				break;
			case PieceType.King:
				kingThreat (x, y, pCol);
				break;
			case PieceType.Pawn:
				pawnThreat (x, y, pCol);
				break;
			}
		}
	}
}
