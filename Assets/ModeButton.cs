using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeButton : MonoBehaviour, IPointerClickHandler {

	public GameManager.GameMode mode;
	public Text text;

	// Use this for initialization
	void Start () {
		string modeName = mode.ToString ();
		int di = -1;
		do {
			string newname = "";
			if (di > -1) {
				newname += modeName.Substring (0, di) + " ";
			}
			newname += modeName.Substring (di + 1, 1).ToUpper() + modeName.Substring (di + 2).ToLower ();
			modeName = newname;
			di = modeName.IndexOf ("_");
		} while(di > -1);
		text.text = modeName;
		GetComponent<Image> ().color = colorForMode (mode);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Color colorForMode(GameManager.GameMode mode) {
		switch (mode) {
		case GameManager.GameMode.FREE_FLY:
			return Color.red;
		case GameManager.GameMode.LIGHTNING:
			return Color.yellow;
		case GameManager.GameMode.MOTION:
			return Color.blue;
		default:
			return Color.black;
		}
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		GameManager.instance.StartGame (mode);
	}

	#endregion
}
