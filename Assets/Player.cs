using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static float SPEED = 0.2f;

	public GameObject eyes;
	public GameObject deadEyes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.Running) {
			float tilt = GameManager.instance.Tilt;
			float x = transform.position.x;
			float y = transform.position.y;
			x += tilt * SPEED;
			if (GameManager.instance.mode == GameManager.GameMode.FREE_FLY) {
				y += GameManager.instance.TiltY * SPEED;
			}
			float screenWidth = GameManager.instance.width;
			if (x < transform.localScale.x / 2 - screenWidth / 2) {
				x = transform.localScale.x / 2 - screenWidth / 2;
			} else if (x > screenWidth / 2 - transform.localScale.x / 2) {
				x = screenWidth / 2 - transform.localScale.x / 2;
			}
			if (y < 0) {
				y = 0;
			}
			transform.localPosition = new Vector3 (x, y, transform.position.z);
		}
	}
}
