﻿using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class ButtonController : MonoBehaviour {

    public GameObject SoundOn;
    public GameObject SoundOff;
    public GameObject ErrorMessage;

	// Use this for initialization
	void Start () {
	    bool sound = PlayerPrefs.GetInt("Sound") > 0 ? false : true;
        if (sound) {
            SoundOff.GetComponent<SpriteRenderer>().enabled = false;
            SoundOn.GetComponent<SpriteRenderer>().enabled = true;
            AudioListener.volume = 1.0f;
        } else {
            SoundOff.GetComponent<SpriteRenderer>().enabled = true;
            SoundOn.GetComponent<SpriteRenderer>().enabled = false;
            AudioListener.volume = 0.0f;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        //
        if (transform.name == "play") {
            if (PlayerPrefs.GetInt("FirstTime") > 0)
                Application.LoadLevel("Main");
            else
                Application.LoadLevel("Tutorial");
        } else if (transform.name == "rank") {
            //
            if (Social.localUser.authenticated)
                Social.ShowLeaderboardUI();
            else {
                Debug.Log ("Error showing social");
                ErrorMessage.SetActive(true);
            }
        } else if (transform.name == "achv") {
            //
            if (Social.localUser.authenticated)
                Social.ShowAchievementsUI();
            else {
                Debug.Log ("Error showing social");
                ErrorMessage.SetActive(true);
            }
        } else if (transform.name == "menu") {
            Application.LoadLevel("Start");
        } else if (transform.name == "rate") {
            //
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.creationguts.jumpy");
        } else if (transform.name.Equals("close")) {
            Application.Quit();
        } else if (transform.name.Equals("errorMessage")) {
            ErrorMessage.SetActive(false);
        }  else if (transform.name.Equals("sound")) {
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
