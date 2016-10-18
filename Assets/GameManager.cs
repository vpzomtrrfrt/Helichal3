using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		GAME_STARTING,
		MODE_MENU,
		INGAME,
		DIED,
		MAIN_MENU,
		PAUSED
	}

	public enum GameMode {
		NORMAL,
		FREE_FLY,
		LIGHTNING,
		MOTION
	}

	public static GameManager instance;

	public float Tilt {
		get {
			return keyTilt () + Input.acceleration.x;
		}
	}

	public float TiltY {
		get {
			return keyTiltY () + Input.acceleration.y;
		}
	}

	public Camera mainCamera;
	public GameObject platformPrefab;
	public GameObject deathMenu;
	public GameObject mainMenu;
	public GameObject modeMenu;
	public GameObject gameOverlay;
	public GameObject youDiedText;
	public GameObject pausedText;

	public GameObject modeButtonPrefab;

	public Text scoreText;
	public Text deathScoreText;
	public Text highScoreText;
	public GameObject titleText;

	public Player player;

	public float width;
	public float screenTop;
	public float screenBottom;
	public float titleTextSpeed;

	public float lastStateSwitch;
	public GameState lastState;
	public float gameStartSpeed;

	public Vector3 menuPlayerPosition;
	public Vector3 startPlayerPosition;

	public Vector3 menuPlayerScale;
	public Vector3 startPlayerScale;

	public bool newHighScore = false;

	private float lastFrameTime;

	private int _score = 0;

	public int Score {
		get {
			return _score;
		}
		set {
			_score = value;
			scoreText.text = "Score: " + value;
			if (value > HighScore) {
				HighScore = value;
			}
		}
	}

	private string HighScoreField {
		get {
			string tr = "HighScore";
			if (mode != GameMode.NORMAL) {
				tr += mode;
			}
			return tr;
		}
	}

	public int HighScore {
		get {
			
			return PlayerPrefs.GetInt (HighScoreField);
		}
		set {
			PlayerPrefs.SetInt (HighScoreField, value);
			highScoreText.text = "High Score: "+value;
			newHighScore = true;
			PlayerPrefs.Save ();
		}
	}

	private GameState _state;

	public GameState State {
		get {
			return _state;
		}
		set {
			lastState = State;
			_state = value;
			if (value == GameState.DIED || value == GameState.PAUSED) {
				deathMenu.SetActive (true);
				if (newHighScore) {
					deathScoreText.text = "NEW HIGH SCORE: " + Score;
					deathScoreText.color = Color.red;
				} else {
					deathScoreText.text = "Score: " + Score;
					deathScoreText.color = Color.black;
				}
				youDiedText.SetActive (value == GameState.DIED);
				pausedText.SetActive (value == GameState.PAUSED);
			} else {
				deathMenu.SetActive (false);
			}
			player.deadEyes.SetActive (value == GameState.DIED);
			player.eyes.SetActive (value != GameState.DIED);
			if (value == GameState.INGAME) {
				gameOverlay.SetActive (true);
			} else {
				gameOverlay.SetActive (false);
			}
			if (value == GameState.MAIN_MENU) {
				Platform.ClearPlatforms ();
				if(lastState != GameState.MODE_MENU) {
					modeMenu.transform.localPosition = new Vector3 (((RectTransform)modeMenu.transform).rect.width, 0, 0);
				}
			}
			mainMenu.SetActive (value == GameState.MAIN_MENU || value == GameState.MODE_MENU);
			modeMenu.SetActive (value == GameState.MAIN_MENU || value == GameState.MODE_MENU);
			lastStateSwitch = Time.time;
		}
	}

	public GameMode mode;

	public bool Running {
		get {
			return State == GameState.INGAME;
		}
	}

	void Awake ()
	{
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
		player.transform.localPosition = menuPlayerPosition;
		State = GameState.MAIN_MENU;
		int y = 1;
		foreach(GameMode mode in Enum.GetValues(typeof(GameMode))) {
			GameObject obj = Instantiate (modeButtonPrefab);
			ModeButton btn = obj.GetComponent<ModeButton> ();
			btn.mode = mode;
			obj.transform.SetParent(modeMenu.transform, false);
			RectTransform rectTransform = (RectTransform)obj.transform;
			obj.transform.localPosition = new Vector3 (0, y*rectTransform.rect.height*1.5f, 0);
			//rectTransform.sizeDelta = new Vector2 (80, 1);
			y--;
		}
		//HighScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lastFrameTime == 0) {
			lastFrameTime = Time.time;
		}
		if (Running) {
			Platform.Generate ();
		} else if (State == GameState.MAIN_MENU) {
			RectTransform transform = titleText.GetComponent<RectTransform> ();
			if (transform.position.x < -2) {
				titleTextSpeed = Mathf.Abs (titleTextSpeed);
			} else if (transform.position.x > 2) {
				titleTextSpeed = -Mathf.Abs (titleTextSpeed);
			}
			transform.position += new Vector3 (titleTextSpeed, 0);
			lerpTo (menuPlayerPosition, menuPlayerScale, player.transform);
			lerpTo (Vector3.zero, mainMenu.transform, gameStartSpeed);
			lerpTo (new Vector3 (((RectTransform)modeMenu.transform).rect.width, 0, 0), modeMenu.transform, gameStartSpeed);
		} else if (State == GameState.GAME_STARTING) {
			player.transform.localRotation = Quaternion.identity;
			float time = lerpTo (startPlayerPosition, startPlayerScale, player.transform, lastState == GameState.MODE_MENU ? gameStartSpeed : gameStartSpeed / 3);
			if (time >= 1) {
				State = GameState.INGAME;
			}
		} else if (State == GameState.MODE_MENU) {
			lerpTo (new Vector3 (-((RectTransform)mainMenu.transform).rect.width, 0, 0), mainMenu.transform, gameStartSpeed);
			lerpTo (Vector3.zero, modeMenu.transform, gameStartSpeed);
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			switch (State) {
			case GameState.DIED:
			case GameState.GAME_STARTING:
			case GameState.MODE_MENU:
				State = GameState.MAIN_MENU;
				break;
			case GameState.INGAME:
				State = GameState.PAUSED;
				break;
			}
		}
		lastFrameTime = Time.time;
	}

	private float keyTilt ()
	{
		float tr = 0;
		if (Input.GetKey (KeyCode.LeftArrow))
			tr--;
		if (Input.GetKey (KeyCode.RightArrow))
			tr++;
		return tr;
	}

	private float keyTiltY ()
	{
		float tr = 0;
		if (Input.GetKey (KeyCode.DownArrow))
			tr--;
		if (Input.GetKey (KeyCode.UpArrow))
			tr++;
		return tr;
	}

	public void YouDied ()
	{
		if (State == GameState.INGAME) {
			State = GameState.DIED;
		}
	}

	public void StartGame() {
		if (State == GameState.PAUSED) {
			State = GameState.INGAME;
		} else {
			StartGame (mode);
		}
	}

	public void StartGame (GameMode mode)
	{
		this.mode = mode;
		State = GameState.GAME_STARTING;
		Score = 0;
		Platform.ClearPlatforms ();
		newHighScore = false;
		highScoreText.text = "High Score: " + HighScore;
	}

	public void SwitchState(string state) {
		SwitchState ((GameState)Enum.Parse (typeof(GameState), state));
	}

	public void SwitchState(GameState state) {
		State = state;
	}

	public void ToMenu() {
		State = GameState.MAIN_MENU;
	}

	private float lerpTo(Vector3 position, Vector3 scale, Transform transform) {
		return lerpTo (position, scale, transform, gameStartSpeed);
	}

	private float lerpTo(Vector3 position, Transform transform, float speed) {
		return lerpTo (position, transform.localScale, transform, speed);
	}

	private float lerpTo(Vector3 position, Vector3 scale, Transform transform, float speed) {
		float t = (Time.time - lastStateSwitch)/speed;
		float lastT = (lastFrameTime - lastStateSwitch) / speed;
		float toPass = (t - lastT)/(1-lastT);
		transform.localPosition = Vector3.Lerp (transform.localPosition, position, toPass);
		transform.localScale = Vector3.Lerp (transform.localScale, scale, toPass);
		return t;
	}

	public void OnApplicationPause(bool paused) {
		if (State == GameState.INGAME) {
			State = GameState.PAUSED;
		}
	}
}
