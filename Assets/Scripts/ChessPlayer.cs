using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class ChessPlayer :NetworkBehaviour
{



	#region  var singleton
	private static ChessPlayer _instance;
	public static ChessPlayer Instance{
		get{ 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(ChessPlayer)) as ChessPlayer;
			}
			return _instance;
		}
	}
	#endregion
	public const int Rows = 15;
	public const int Columns = 15;
	public int[,] ChessGrid=new int[Rows,Columns];
	public  Vector2 chessSize = Vector2.zero;
	public float chessGridWidth=0.0f;
	public UnityEngine.RectTransform calculateChessBoard;
	public GameObject whiteChessPrefab;
	public void PlaceChessPos(RectTransform calculateChessBoard,Vector2 v,GameObject whiteChessPrefab)
	{
		if (!isLocalPlayer)
			return;
		this.calculateChessBoard = calculateChessBoard;
		this.whiteChessPrefab = whiteChessPrefab;
		chessSize = calculateChessBoard.rect.size;
		chessGridWidth = chessSize.x / 14;
		int _x = Mathf.RoundToInt (v.x / chessGridWidth);
		int _y = Mathf.RoundToInt (v.y / chessGridWidth);
		CmdPlaceChess (_x, _y);


	}


	[Command]
	private void CmdPlaceChess(int _x,int _y){
		ChessGrid [_x, _y] = 1;
		GameObject go = GameObject.Instantiate (whiteChessPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.SetParent (calculateChessBoard.transform,false);
		go.SetActive (true);
		go.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_x * chessGridWidth, _y * chessGridWidth);
		NetworkServer.Spawn (go);
	}

}
