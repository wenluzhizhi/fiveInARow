using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Net;
using DG.Tweening;




public class MianUI :UIBehaviour{


	#region  singleton
	private static MianUI _instance;
	public static MianUI Instance{
		get{ 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(MianUI)) as MianUI;
			}
			return _instance;
		}
	}


	#endregion


	#region  var
	public RectTransform calculateChessBoard;
	public Canvas ca;
	public Vector2 ScreenPos;


	public Vector2 PressedPos=Vector2.zero;

	public ChessPlayer player;

	public int PlayerCount=0;




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

	#endregion

	#region  monoEvent

	protected override void Start ()
	{
		base.Start ();
		whiteChessPrefab.gameObject.SetActive (false);
		blackChessPrefab.gameObject.SetActive (false);
		currentPlayerCountText.text = "当前在线：0";
		InitGrid ();
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

	#endregion


	#region external funcion
	public void AddPlayer(int k)
	{
		PlayerCount+=k;
		if (PlayerCount >= 2) {
			StartGame.interactable = true;
		}
		currentPlayerCountText.text = "在线："+PlayerCount;
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


	public void StopGame(){
		OnClickStopGame ();
		InitGrid ();
	}
	#endregion

	#region  OnClick Event
	public void OnClickStarGame()
	{
		if (player != null)
		{
			player.StartInitGame ();
		}
		InitGrid ();
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
		RectTransform _t = SetUpPanel.gameObject.GetComponent<RectTransform> ();
		if (_t.gameObject.activeInHierarchy) {
			_t.DOScaleY (0.0f, 0.2f).OnComplete (delegate() {
				_t.gameObject.SetActive (false);

			});
		} else {
			_t.gameObject.SetActive (true);
			_t.DOScaleY (1.0f, 0.2f);
		}
	}

	public void ShowLog(){
		ShowLogConsole.visible = !ShowLogConsole.visible;
	}

	public void OnClickShowOperations()
	{
		RectTransform _t = OperationTips.gameObject.GetComponent<RectTransform> ();
		if (_t.gameObject.activeInHierarchy) {
			_t.DOScaleY (0.0f, 0.2f).OnComplete (delegate() {
				_t.gameObject.SetActive (false);
				
			});
		} else {
			_t.gameObject.SetActive (true);
			_t.DOScaleY (1.0f, 0.2f);
		}
	}

	public void  GetLocalIP(){
		string hostName = Dns.GetHostName ();
		IPHostEntry localHost = Dns.GetHostEntry (hostName);
		IPAddress[] localAdd = localHost.AddressList;
		ipText.text = "";
		for (int i = 0; i < localAdd.Length; i++) {
			ipText.text += localAdd [i].ToString () + "\r\n";
		}

	}

	public void CaculateResult(int x,int y,int type){
		Debug.Log ("type-------"+type);
		#region 水平
		int su_c=1;
		int _k = x-1;
		while (_k > 0 && chessGridPos [_k, y] == type) {
			su_c++;
			_k = _k - 1;
		}

		_k = x+1;
		while (_k <Columns && chessGridPos [_k, y] == type) {
			su_c++;
			_k = _k+ 1;
		}


		if (su_c >= 5) {
			Debug.Log (type+"___win:"+su_c);
			MainController.Instance.ShowResult(type);
			return;
		}
		#endregion


		#region 垂直
	    su_c=1;
	    _k = y-1;
		while (_k > 0 && chessGridPos [x, _k] == type) {
			su_c++;
			_k = _k - 1;
		}

		_k = y+1;
		while (_k <Columns && chessGridPos [x, _k] == type) {
			su_c++;
			_k = _k+ 1;
		}


		if (su_c >= 5) {
			Debug.Log (type+"___win");
			MainController.Instance.ShowResult(type);
			return;
		}
		#endregion

	
		#region  斜45
		su_c=1;
		_k = y-1;
		int _k2=x-1;
		while (_k > 0 &&_k2>0&& chessGridPos [_k2, _k] == type) {
			su_c++;
			_k2 = _k2 - 1;
			_k=_k-1;
		}

		_k = y+1;
		_k2=x+1;
		while (_k2<ROWS&&_k <Columns && chessGridPos [_k2, _k] == type) {
			su_c++;
			_k = _k+ 1;
			_k2=_k2+1;
		}


		if (su_c >= 5) {
			Debug.Log (type+"___win");
			MainController.Instance.ShowResult(type);
			return;
		}

		#endregion
	
		#region  反斜45

		su_c=1;
		_k = y+1;
	    _k2=x-1;
		while (_k < ROWS &&_k2>0&& chessGridPos [_k2, _k] == type) {
			su_c++;
			_k2 = _k2 - 1;
			_k=_k+1;
		}

		_k = y-1;
		_k2=x+1;
		while (_k2<Columns&&_k >0 && chessGridPos [_k2, _k] == type) {
			su_c++;
			_k = _k-1;
			_k2=_k2+1;
		}


		if (su_c >= 5) {
			Debug.Log (type+"___win");
			MainController.Instance.ShowResult(type);
			return;
		}

		#endregion
	
	}



	#endregion

	#region  internal function

	private void InitGrid(){
		for (int i = 0; i < ROWS; i++) {
			for (int j = 0; j < Columns; j++) {
				chessGridPos [i, j] = 0;
			}
		}
	}

	#endregion
}
