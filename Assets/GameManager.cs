using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		GAME_STARTING,

		INGAME,
		DIED,
		MAIN_MENU
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
	public GameObject mainMenu;
	public GameObject gameOverlay;

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

	public Vector3 sourcePosition;
	public Vector3 sourceScale;

	public bool newHighScore = false;

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

	public int HighScore {
		get {
			return PlayerPrefs.GetInt ("HighScore");
		}
		set {
			PlayerPrefs.SetInt ("HighScore", value);
			highScoreText.text = "High Score: "+value;
			newHighScore = true;
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
			if (value == GameState.DIED) {
				deathMenu.SetActive (true);
				if (newHighScore) {
					deathScoreText.text = "NEW HIGH SCORE: " + Score;
					deathScoreText.color = Color.red;
				} else {
					deathScoreText.text = "Score: " + Score;
					deathScoreText.color = Color.black;
				}
			} else {
				deathMenu.SetActive (false);
			}
			if (value == GameState.INGAME) {
				gameOverlay.SetActive (true);
			} else {
				gameOverlay.SetActive (false);
			}
			if (value == GameState.MAIN_MENU) {
				mainMenu.SetActive (true);
				sourcePosition = player.transform.position;
				sourceScale = player.transform.localScale;
				Platform.ClearPlatforms ();
			} else {
				mainMenu.SetActive (false);
			}
			lastStateSwitch = Time.time;
		}
	}

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
		highScoreText.text = "High Score: " + HighScore;
		//HighScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Running) {
			Platform.Generate ();
		} else if (State == GameState.MAIN_MENU) {
			RectTransform transform = titleText.GetComponent<RectTransform> ();
			Rect rect = transform.rect;
			if (transform.position.x < -2) {
				titleTextSpeed = Mathf.Abs (titleTextSpeed);
			} else if (transform.position.x > 2) {
				titleTextSpeed = -Mathf.Abs (titleTextSpeed);
			}
			transform.position += new Vector3 (titleTextSpeed, 0);
			lerpTo (menuPlayerPosition, menuPlayerScale, player.transform);
		} else if (State == GameState.GAME_STARTING) {
			player.transform.localRotation = Quaternion.identity;
			float time = lerpTo (startPlayerPosition, startPlayerScale, player.transform, lastState==GameState.MAIN_MENU?gameStartSpeed:gameStartSpeed/3);
			if (time >= 1) {
				State = GameState.INGAME;
			}
		}
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

	public void YouDied ()
	{
		if (State == GameState.INGAME) {
			State = GameState.DIED;
		}
	}

	public void StartGame ()
	{
		State = GameState.GAME_STARTING;
		Score = 0;
		Platform.ClearPlatforms ();
		sourcePosition = player.transform.position;
		sourceScale = player.transform.localScale;
		newHighScore = false;
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

	private float lerpTo(Vector3 position, Vector3 scale, Transform transform, float speed) {
		float t = (Time.time - lastStateSwitch)/speed;
		transform.localPosition = Vector3.Lerp (sourcePosition, position, t);
		transform.localScale = Vector3.Lerp (sourceScale, scale, t);
		return t;
	}
}
