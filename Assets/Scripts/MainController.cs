using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MainController : NetworkBehaviour
{


	#region  singleton
	private static MainController _instance;
	public static MainController Instance{
		get{ 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(MainController)) as MainController;
			}
			return _instance;
		}
	}
	#endregion

	[SyncVar]  
	public int chessCount = 0;

	[SyncVar]
	public float timer=0.0f;
	void Start()
	{
		Debug.Log ("MainController shengc ....."+Time.time);

		MianUI.Instance.StartRealGame ();
	}

	public void AddChessCount(){
		chessCount++;
		timer = 0.0f;
	}

	public bool CheckIsMyPlayerPlace(int id){
		if (id == 1) {
			if (chessCount % 2 == 0) {
				return true;
			}
		} 
		else if(id==2) 
		{
			if (chessCount % 2 == 1) {
				return true;
			}
			
		}
		return false;
	}


	public void ShowResult(int type){
		RpcshowEnd (type);
	}


	[ClientRpc]
	void RpcshowEnd(int type){

		Debug.Log ("--------"+type+"    win");
		if (MianUI.Instance.player.PlayerID == type) {
			TipManager.Instance.ShowTips ("you are win");
		} else {
			TipManager.Instance.ShowTips ("you are lost");
		}

	}

}
