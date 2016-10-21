using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeButton : MonoBehaviour, IPointerClickHandler {

	public GameManager.GameMode mode;
	public Text text;
    public bool forCustom;

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
		modeName += " Mode";
	    if (forCustom)
	    {
	        modeName += GetPunctuation();
	    }
		text.text = modeName;
		text.color = Color.white;
		GetComponent<Image> ().color = colorForMode (mode);
	}

    private string GetPunctuation()
    {
        return GameManager.instance.customModes.Contains(mode)?"!":"?";
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

    public void OnPointerClick (PointerEventData eventData)
	{
	    if (forCustom)
	    {
	        if (!GameManager.instance.customModes.Remove(mode))
	        {
	            GameManager.instance.customModes.Add(mode);
	        }
	        text.text = text.text.Substring(0, text.text.Length - 1) + GetPunctuation();
	    }
	    else if (mode == GameManager.GameMode.CUSTOM)
	    {
	        GameManager.instance.State = GameManager.GameState.CUSTOM_MENU;
	    }
	    else
	    {
	        GameManager.instance.StartGame(mode);
	    }
	}
}
