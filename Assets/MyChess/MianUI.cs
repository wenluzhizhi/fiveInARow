using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Net;




public class MianUI :UIBehaviour{

	public RectTransform calculateChessBoard;
	public Canvas ca;
	public Vector2 ScreenPos;


	public Vector2 PressedPos=Vector2.zero;

	public ChessPlayer player;

	public int PlayerCount=0;


	private static MianUI _instance;
	public static MianUI Instance{
		get{ 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(MianUI)) as MianUI;
			}
			return _instance;
		}
	}


	public GameObject whiteChessPrefab;
	public GameObject blackChessPrefab;


	public const int ROWS = 15;
	public const int Columns = 15;
	public int[,] chessGridPos=new int[ROWS,Columns];

	public GameObject SetUpPanel;
	public Button StartGame;
	public Button StartHost;
	public Button joinGame;
	public Image blackChess;
	public Image whiteChess;
	public Text currentPlayerCountText;
	public Text currentState;
	public InputField IpField;
	public Text ipText;
	public GameObject OperationTips;

	protected override void Start ()
	{
		base.Start ();
		whiteChessPrefab.gameObject.SetActive (false);
		blackChessPrefab.gameObject.SetActive (false);
		currentPlayerCountText.text = "当前在线：0";
	}

	public void AddPlayer(int k)
	{
		PlayerCount+=k;
		if (PlayerCount >= 2) {
			StartGame.interactable = true;
		}
		currentPlayerCountText.text = "在线："+PlayerCount;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape)){  
			Application.Quit();  
	    }   
		if(MainController.Instance==null)
			return;
		if (MainController.Instance.CheckIsMyPlayerPlace (player.PlayerID)) 
		{
			MainController.Instance.timer += Time.deltaTime;
			currentState.text = "思考中:"+Mathf.RoundToInt(MainController.Instance.timer)+"秒";
		} 
		else 
		{
			MainController.Instance.timer += Time.deltaTime;
			currentState.text = "等待中:"+Mathf.RoundToInt(MainController.Instance.timer)+"秒";
		}
		if (Input.GetMouseButtonDown (0))
		{
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (calculateChessBoard, Input.mousePosition, ca.worldCamera, out v);
			ScreenPos = v;
			Vector2 size =calculateChessBoard.rect.size;
			float _gridWidth = size.x / 14.0f;
			int x = Mathf.RoundToInt (v.x / _gridWidth);
			int y = Mathf.RoundToInt (v.y / _gridWidth);
			if (player != null) 
			{
					player.PlaceChessPos (x, y);
				  

			}
		}
	}

	public void StartRealGame(){
		StartGame.interactable = false;
		joinGame.interactable = false;
		StartHost.interactable = false;

		//此处开始真正的游戏
		if(player.PlayerID==1){
			blackChess.gameObject.SetActive (true);
			whiteChess.gameObject.SetActive (false);
		}
		else if(player.PlayerID==2){
			blackChess.gameObject.SetActive (false);
			whiteChess.gameObject.SetActive (true);
		}
	}

	#region  OnClick Event
	public void OnClickStarGame()
	{
		if (player != null)
		{
			player.StartInitGame ();
		}
	}
	public void OnClickStartHost()
	{
		joinGame.interactable = false;
		NetworkManager.singleton.StartHost ();
	}


	public void OnClickJoinGame(){
		StartHost.interactable = false;
		NetworkManager.singleton.StartClient ();
	}

	public void OnClickStopGame(){
		StartGame.interactable = true;
		StartHost.interactable = true;
		joinGame.interactable = true;
		NetworkManager.singleton.StopHost ();

	}


	public void SetIP(){
		string ip=IpField.text.Trim ();
		NetworkManager.singleton.networkAddress = ip;
	}

	public void OnClickSetUpPanel(){
		SetUpPanel.gameObject.SetActive (!SetUpPanel.activeInHierarchy);
	}

	public void ShowLog(){
		ShowLogConsole.visible = !ShowLogConsole.visible;
	}

	public void OnClickShowOperations(){
		OperationTips.gameObject.SetActive (!OperationTips.gameObject.activeInHierarchy);
	}
	#endregion

	public void  GetLocalIP(){
		string hostName = Dns.GetHostName ();
		IPHostEntry localHost = Dns.GetHostEntry (hostName);
		IPAddress[] localAdd = localHost.AddressList;
		ipText.text = "";
		for (int i = 0; i < localAdd.Length; i++) {
			ipText.text += localAdd [i].ToString () + "\r\n";
		}
	
	}

}
