using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour {

	public Transform leftPiece;
	public Transform rightPiece;

	public float speed;

	public static List<Platform> platforms = new List<Platform>();

	private float _x = 0;

	public float x {
		set {
			float screenWidth = GameManager.instance.width;
			float leftWidth = screenWidth / 2 + value - screenWidth/6;
			float rightWidth = screenWidth*2 / 3 - leftWidth;
			leftPiece.localPosition = new Vector3 (-screenWidth/2+leftWidth/2, 0, 0);
			leftPiece.localScale = new Vector3 (leftWidth, leftPiece.localScale.y, leftPiece.localScale.z);
			rightPiece.localPosition = new Vector3 (screenWidth/2-rightWidth/2, 0, 0);
			rightPiece.localScale = new Vector3 (rightWidth, rightPiece.localScale.y, rightPiece.localScale.z);
			_x = value;
		}
		get {
			return _x;
		}
	}

	private float y;

	private float xv = 1;

	void Awake() {
		platforms.Add (this);
	}

	// Use this for initialization
	void Start () {
		if (_x == 0) {
			x = 0;
		}
		y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.Running) {
			y -= speed*Player.Speed;
			transform.localPosition = new Vector3 (transform.position.x, y, transform.position.z);
			if (y + transform.localScale.y / 2 < GameManager.instance.screenBottom) {
				Destroy (gameObject);
				GameManager.instance.Score++;
				platforms.Remove (this);
			}
			if (GameManager.instance.IsModeEffective(GameManager.GameMode.MOTION)) {
				x += xv*speed*Player.Speed;
				if (x > GameManager.instance.width /3) {
					xv = -Math.Abs (xv);
				} else if (x < -GameManager.instance.width/3) {
					xv = Math.Abs (xv);
				}
			}
		}
	}


	public static void Generate ()
	{
		float screenWidth = GameManager.instance.width;
		if (platforms.Count > 0) {
			Platform lastPlatform = platforms [platforms.Count - 1];
			if (lastPlatform.transform.position.y < Math.Max(GameManager.instance.screenTop, GameManager.instance.player.transform.position.y)) {
				float npx = UnityEngine.Random.value * screenWidth * 2 / 3 - screenWidth / 2 + screenWidth / 6;
				float dsc = (GameManager.instance.Score + platforms.Count) / 200;
				float dist = Math.Abs (npx - lastPlatform.x);
				float playerPart = 1 - dsc / 10;
				float distPart = dist / (Player.Speed * 5);
				float randomPart = UnityEngine.Random.value / 6;
				Platform platform = Create ();
				float nph = lastPlatform.transform.position.y + Math.Max (platform.transform.localScale.y, Math.Min (playerPart + distPart + randomPart, 12));
				platform.transform.localPosition = new Vector3 (platform.transform.position.x, nph, platform.transform.position.z);
				platform.x = npx;
			}
		} else {
			Platform platform = Create ();
			platform.transform.localPosition = new Vector3 (0, 3, 0);
		}
	}

	public static void ClearPlatforms ()
	{
		while (platforms.Count > 0) {
			Platform platform = platforms [0];
			platforms.Remove (platform);
			Destroy (platform.gameObject);
		}
	}

	private static Platform Create ()
	{
		return ((GameObject)Instantiate (GameManager.instance.platformPrefab)).GetComponent<Platform> ();
	}
}
