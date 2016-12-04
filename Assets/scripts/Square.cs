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

	public void Clear() {
		GetComponent<Image> ().color = color_;
		occupied = false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
