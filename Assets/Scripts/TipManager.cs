using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipManager : MonoBehaviour 
{



	#region  singleton

	private static TipManager _instance;
	public static TipManager Instance{
		get{ 
			if (_instance == null) {
				_instance=GameObject.FindObjectOfType(typeof(TipManager)) as TipManager;
			}
			return _instance;
		}
	}


	#endregion


	#region  var

	[SerializeField] private GameObject  tips;
	[SerializeField] private Text showTex;

	#endregion



	#region  external function

	public void ShowTips(string str){
		tips.SetActive (true);
		showTex.text = str;
	}




	#endregion



	#region OnclcikEvent

	public void OnClickCloseTips()
	{
		tips.gameObject.SetActive (false);
		MianUI.Instance.StopGame ();
	}


	#endregion


}
