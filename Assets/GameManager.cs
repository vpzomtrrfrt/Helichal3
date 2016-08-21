using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

	public float tilt = 0;

	public Camera mainCamera;

	public float width;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		Vector3 lowerLeft = mainCamera.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		Vector3 upperRight = mainCamera.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
		width = upperRight.x - lowerLeft.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			tilt += 1;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			tilt -= 1;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			tilt -= 1;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			tilt += 1;
		}
	}
}
