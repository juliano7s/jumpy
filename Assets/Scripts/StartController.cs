using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class StartController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //googlePlayDebug.GetComponent<TextMesh>().text = "authenticating on google play";
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {            
            //googlePlayDebug.GetComponent<TextMesh>().text = "User authenticate google play games: " + success;
        });	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
