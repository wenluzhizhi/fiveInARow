using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class ChessPlayer :NetworkBehaviour
{





	public const int Rows = 15;
	public const int Columns = 15;
	public int[,] ChessGrid=new int[Rows,Columns];
	public  Vector2 chessSize = Vector2.zero;
	public float chessGridWidth=0.0f;
	public UnityEngine.RectTransform calculateChessBoard;
	public GameObject whiteChessPrefab;

	[SyncVar]
	public int PlayerID = 0;

	void Start()
	{
		if (isLocalPlayer) {
			this.gameObject.name = "localPlayer";
			MianUI.Instance.player = this;
			whiteChessPrefab = MianUI.Instance.whiteChessPrefab;
		} 
		else 
		{
			this.gameObject.name = "enemy";
		}
		MianUI.Instance.AddPlayer (1);
		if (isServer) {
			PlayerID  = MianUI.Instance.PlayerCount;
		}

	}

	void OnDestroy(){
		if(MianUI.Instance!=null)
	     	MianUI.Instance.AddPlayer (-1);
	}

	public void StartInitGame(){
		if (MainController.Instance == null) {
			Debug.Log ("StartInitGame---");
			CmdAddController ();
		}
	}
	[Command]
	public void CmdAddController(){
		Debug.Log ("CmdAddController---"+Time.time);
		GameObject go = GameObject.Instantiate (NetworkManager.singleton.spawnPrefabs [2]);
		NetworkServer.Spawn (go);
	}

	public void PlaceChessPos(int x,int y)
	{
		if (!isLocalPlayer)
			return;
		if (MainController.Instance.CheckIsMyPlayerPlace (PlayerID)) {
			CmdPlaceChess (x, y);
		}


	}

	int chessType=2;  //1黑棋，2 白棋

	[Command]
	private void CmdPlaceChess(int x,int y)
	{
		Debug.Log ("Server excute CmdPlaceChess---"+x+","+y+"  "+this.gameObject.name);
		if (MianUI.Instance.chessGridPos [x, y] == 0) 
		{
			GameObject p = MianUI.Instance.whiteChessPrefab;
			if (PlayerID % 2 == 1) {
				p = MianUI.Instance.blackChessPrefab;
				chessType = 2;

			} else {
				chessType = 1;
			}
			MainController.Instance.AddChessCount ();
			GameObject go = GameObject.Instantiate (p, Vector3.zero, Quaternion.identity) as GameObject;
			go.GetComponent<Chessdot> ().SetXY (x, y);
			go.SetActive (true);
			NetworkServer.Spawn (go);
			MianUI.Instance.chessGridPos [x, y] = chessType;
			MianUI.Instance.CaculateResult (x, y, chessType);
		}
	}





}
