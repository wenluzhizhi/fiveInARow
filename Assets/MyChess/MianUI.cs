using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;




public class MianUI :UIBehaviour{

	public RectTransform calculateChessBoard;
	public Canvas ca;
	public Vector2 ScreenPos;


	public Vector2 PressedPos=Vector2.zero;

	[SerializeField] private GameObject whiteChessPrefab;
	[SerializeField] private GameObject blackChessPrefab;


	void Start () 
	{
		whiteChessPrefab.gameObject.SetActive (false);
		blackChessPrefab.gameObject.SetActive (false);

	}


	void Update () 
	{
		if (Input.GetMouseButtonDown (0))
		{
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (calculateChessBoard, Input.mousePosition, ca.worldCamera, out v);
			ScreenPos = v;
			if (ChessPlayer.Instance != null) {
				ChessPlayer.Instance.PlaceChessPos (calculateChessBoard, v, whiteChessPrefab);
			}
		}
	}


}
