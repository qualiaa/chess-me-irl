using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Square : MonoBehaviour {

	public Dictionary<PieceColor,bool> threatenedBy;
	public bool occupied = false;

	Color color_;

	void Start () {
		threatenedBy = new Dictionary<PieceColor, bool>();
		threatenedBy [PieceColor.Black] = false;
		threatenedBy [PieceColor.White] = false;

		color_ = GetComponent<Image> ().color;
	}

	public void Clear()
	{
		GetComponent<Image> ().color = color_;
		occupied = false;
		threatenedBy [PieceColor.Black] = false;
		threatenedBy [PieceColor.White] = false;
	}

	public void SetColor(Color c)
	{
		c.a = 0.35f;
		GetComponent<Image> ().color = c;
	}

	public void SetThreat(PieceColor c, bool t = true)
	{
		threatenedBy [c] = t;
	}
}
