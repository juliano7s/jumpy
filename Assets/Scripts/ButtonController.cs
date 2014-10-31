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
        Application.LoadLevel("Main");
    }
}
