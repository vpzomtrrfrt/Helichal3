using UnityEngine;
using System.Collections;
using System;

public class Pupil : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float tilt = GameManager.instance.tilt;
		Transform transform = GetComponent<Transform> ();
		float x = tilt/4;
		float y = (1 - Math.Abs(tilt)) / 5;
		float z = transform.position.z;
		transform.localPosition = new Vector3 (x, y, z);
	}
}
