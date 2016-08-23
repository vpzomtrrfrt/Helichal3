using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static float SPEED = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.Running) {
			float tilt = GameManager.instance.Tilt;
			float x = transform.position.x;
			x += tilt * SPEED;
			float screenWidth = GameManager.instance.width;
			if (x < transform.localScale.x / 2 - screenWidth / 2) {
				x = transform.localScale.x / 2 - screenWidth / 2;
			} else if (x > screenWidth / 2 - transform.localScale.x / 2) {
				x = screenWidth / 2 - transform.localScale.x / 2;
			}
			transform.localPosition = new Vector3 (x, transform.position.y, transform.position.z);
		}
	}

	public void OnCollisionEnter(Collision col) {
		Debug.Log ("collision?");
		if (col.gameObject.GetComponent<Deadly> () != null) {
			Debug.Log ("YOU SHOULD BE DEAD");
			GameManager.instance.YouDied ();
		}
	}
}
