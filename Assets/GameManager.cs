using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

	public float Tilt {
		get {
			return keyTilt () + Input.acceleration.x;
		}
	}

	public Camera mainCamera;
	public GameObject platformPrefab;

	public float width;
	public float screenTop;

	public int score = 0;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		Vector3 lowerLeft = mainCamera.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		Vector3 upperRight = mainCamera.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
		width = upperRight.x - lowerLeft.x;
		screenTop = upperRight.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Platform.Generate ();
	}

	private float keyTilt() {
		float tr = 0;
		if (Input.GetKey (KeyCode.LeftArrow))
			tr--;
		if (Input.GetKey (KeyCode.RightArrow))
			tr++;
		return tr;
	}
}
