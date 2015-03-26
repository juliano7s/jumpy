using UnityEngine;
using System.Collections;

public class SpeechBalloonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		if (transform.rotation != Quaternion.Euler(0, 0, 0)) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
    }
}
