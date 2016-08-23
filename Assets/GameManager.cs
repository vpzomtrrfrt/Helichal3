using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		INGAME,
		DIED
	}

	public static GameManager instance;

	public float Tilt {
		get {
			return keyTilt () + Input.acceleration.x;
		}
	}

	public Camera mainCamera;
	public GameObject platformPrefab;
	public GameObject deathMenu;
	public Text scoreText;
	public Text deathScoreText;
	public Player player;

	public float width;
	public float screenTop;
	public float screenBottom;

	private int _score = 0;

	public int Score {
		get {
			return _score;
		}
		set {
			_score = value;
			scoreText.text = "Score: " + value;
		}
	}

	private GameState _state;

	public GameState State {
		get {
			return _state;
		}
		set {
			_state = value;
			if (value == GameState.DIED) {
				deathMenu.SetActive (true);
				deathScoreText.text = "Score: " + Score;
			} else {
				deathMenu.SetActive (false);
			}
			if (value == GameState.INGAME) {
				Score = 0;
				player.transform.localPosition = new Vector3 (0, 0, 0);
				player.transform.localRotation = new Quaternion (0, 0, 0, 0);
			}
		}
	}

	public bool Running {
		get {
			return State == GameState.INGAME;
		}
	}

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
		screenBottom = lowerLeft.y;
		State = GameState.INGAME;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Running) {
			Platform.Generate ();
		}
	}

	private float keyTilt() {
		float tr = 0;
		if (Input.GetKey (KeyCode.LeftArrow))
			tr--;
		if (Input.GetKey (KeyCode.RightArrow))
			tr++;
		return tr;
	}

	public void YouDied() {
		if(State == GameState.INGAME) {
			State = GameState.DIED;
		}
	}

	public void StartGame() {
		State = GameState.INGAME;
	}
}
