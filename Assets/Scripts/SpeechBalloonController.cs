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
        Vector3 parentPos = transform.parent.gameObject.transform.position;
        transform.position = new Vector3(parentPos.x + 5.0f, parentPos.y + 6.4f, transform.position.z);
    }
}
