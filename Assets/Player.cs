using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float tilt = GameManager.instance.tilt;
		float x = transform.position.x;
		x += tilt/4;
		float screenWidth = GameManager.instance.width;
		if (x < transform.localScale.x/2-screenWidth/2) {
			x = transform.localScale.x/2-screenWidth/2;
		} else if(x > screenWidth/2-transform.localScale.x/2) {
			x = screenWidth/2-transform.localScale.x/2;
		}
		transform.localPosition = new Vector3 (x, transform.position.y, transform.position.z);
	}
}
