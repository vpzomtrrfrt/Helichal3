using UnityEngine;
using System.Collections;

public class Deadly : MonoBehaviour
{
	void Update() {
		if (GetComponent<Renderer> ().bounds.Intersects (GameManager.instance.player.GetComponent<Renderer> ().bounds)) {
			GameManager.instance.YouDied ();
		}
	}
}
