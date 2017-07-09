using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Chessdot :NetworkBehaviour {


	[SyncVar]
	public int X;
	[SyncVar]
	public int Y;



	void Start()
	{
		//Debug.Log ("Chessdot --Start---"+this.gameObject.name+"  "+Time.time);
		SetPos();
	}
	public void SetXY(int x,int y){

		//Debug.Log ("Chessdot --SetXY---"+this.gameObject.name+"  "+Time.time);
		this.X = x;
		this.Y = y;
	}
	[ContextMenu("Reset")]
	public void SetPos(){
		//Debug.Log ("Chessdot --Setpos---"+this.gameObject.name+"  "+Time.time+" X="+X+" y"+Y);
		float gridWidth = MianUI.Instance.calculateChessBoard.rect.size.x / 14.0f;
		float x1 = X * gridWidth;
		float y1 = Y * gridWidth;
		this.gameObject.transform.SetParent (MianUI.Instance.calculateChessBoard,false);
		this.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x1,y1);
	}

}
