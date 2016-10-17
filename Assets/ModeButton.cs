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
		modeName = modeName [0] + modeName.Substring (1).ToLower ()+" Mode";
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
