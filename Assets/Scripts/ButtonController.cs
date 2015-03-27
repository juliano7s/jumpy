using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class ButtonController : MonoBehaviour {

    public GameObject SoundOn;
    public GameObject SoundOff;

	// Use this for initialization
	void Start () {
	    bool sound = PlayerPrefs.GetInt("Sound") > 0 ? false : true;
        if (sound) {
            SoundOff.GetComponent<SpriteRenderer>().enabled = false;
            SoundOn.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            SoundOff.GetComponent<SpriteRenderer>().enabled = true;
            SoundOn.GetComponent<SpriteRenderer>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Debug.Log("Mouse down on " + transform.name);
        if (transform.name == "play")
            Application.LoadLevel("Main");
        else if (transform.name == "rank") {
            Debug.Log("Open rank");
            Social.ShowLeaderboardUI();
        } else if (transform.name == "menu")
            Application.LoadLevel("Start");
        else if (transform.name == "rate") {
            Debug.Log("Open rate");
            Application.OpenURL("market://details?id=com.creationguts.jumpy/");
        } else if (transform.name.Equals("sound")) {
            if (SoundOn.GetComponent<SpriteRenderer>().enabled) {
                SoundOff.GetComponent<SpriteRenderer>().enabled = true;
                SoundOn.GetComponent<SpriteRenderer>().enabled = false;
                PlayerPrefs.SetInt("Sound",1);
                AudioListener.volume = 0.0f;
            } else if (SoundOff.GetComponent<SpriteRenderer>().enabled) {
                SoundOff.GetComponent<SpriteRenderer>().enabled = false;
                SoundOn.GetComponent<SpriteRenderer>().enabled = true;
                PlayerPrefs.SetInt("Sound",0);
                AudioListener.volume = 1.0f;
            }
        }        
    }
}
