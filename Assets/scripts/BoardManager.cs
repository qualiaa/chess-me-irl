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

		squares_[x,y].SetColor(c);
	}

	void SetThreat(int x, int y, PieceColor c)
	{
		if (x < 0 || y < 0 || x >= 8 || y >= 8) {
			return;
		}

		squares_ [x, y].SetThreat(c);
	}

	void bishopThreat(int x, int y, PieceColor c)
	{
		for (int i = 1; i < 8; ++i) {
			SetThreat (x - i, y - i, c);
			if (isOccupied(x-i,y-i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x - i, y + i, c);
			if (isOccupied(x-i,y+i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x + i, y - i, c);
			if (isOccupied(x+i,y-i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x + i, y + i, c);
			if (isOccupied(x+i,y+i)) break;
		}
	}

	void rookThreat(int x, int y, PieceColor c)
	{
		for (int i = 1; i < 8; ++i) {
			SetThreat (x - i, y, c);
			if (isOccupied(x - i, y)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x + i, y, c);
			if (isOccupied(x + i, y)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x, y - i, c);
			if (isOccupied(x, y - i)) break;
		}
		for (int i = 1; i < 8; ++i) {
			SetThreat (x, y + i, c);
			if (isOccupied(x, y + i)) break;
		}
	}

	void knightThreat(int x, int y, PieceColor c)
	{
		SetThreat (x - 2, y - 1, c);
		SetThreat (x - 2, y + 1, c);
		SetThreat (x + 2, y - 1, c);
		SetThreat (x + 2, y + 1, c);
		SetThreat (x - 1, y - 2, c);
		SetThreat (x - 1, y + 2, c);
		SetThreat (x + 1, y - 2, c);
		SetThreat (x + 1, y + 2, c);
	}

	void kingThreat(int x, int y, PieceColor c)
	{
		SetThreat (x + 1, y - 1, c);
		SetThreat (x + 1, y + 1, c);
		SetThreat (x - 1, y - 1, c);
		SetThreat (x - 1, y + 1, c);
		SetThreat (x    , y - 1, c);
		SetThreat (x - 1, y    , c);
		SetThreat (x    , y + 1, c);
		SetThreat (x + 1, y    , c);
	}

	void pawnThreat(int x, int y, PieceColor c)
	{
		int y1 = c == PieceColor.Black ? y + 1 : y - 1;
		SetThreat (x - 1, y1, c);
		SetThreat (x + 1, y1, c);
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

		foreach (var pieceGO in pieces_) {
			var fBoardPos = pieceGO.transform.position;
			fBoardPos.x += 3.5f;
			fBoardPos.y *= -1f;
			fBoardPos.y += 3.5f;
			int x = (int)Mathf.Round (fBoardPos.x);
			int y = (int)Mathf.Round (fBoardPos.y);
			setOccupied (x, y);
		}

		foreach (var pieceGO in pieces_)
		{
			Piece piece = pieceGO.GetComponent<Piece> ();

			var fBoardPos = pieceGO.transform.position;
			fBoardPos.x += 3.5f;
			fBoardPos.y *= -1f;
			fBoardPos.y += 3.5f;
			int x = (int)Mathf.Round(fBoardPos.x);
			int y = (int)Mathf.Round(fBoardPos.y);

			PieceColor pCol = piece.pieceColor;
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
		/*
		foreach (var square in squares_) {
			if (square.threatenedBy[PieceColor.Black] &&
				square.threatenedBy[PieceColor.White]) {
				square.SetColor(Color.clear);
			}
			else if (square.threatenedBy[PieceColor.Black]) {
				square.SetColor(Color.black);
			}
			else if (square.threatenedBy[PieceColor.White]) {
				square.SetColor(Color.white);
			}
		}
		*/
	}
}
