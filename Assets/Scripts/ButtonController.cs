using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Debug.Log("Mouse down on " + transform.name);
        if (transform.name == "play")
            Application.LoadLevel("Main");
        else if (transform.name == "rank")
            Debug.Log("Open rank");
        else if (transform.name == "menu")
            Application.LoadLevel("Start");
        else if (transform.name == "rate")
            Debug.Log("Open rate");
    }
}
