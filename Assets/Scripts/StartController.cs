using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class StartController : MonoBehaviour {

    public GameObject ErrorMessage;

	// Use this for initialization
	void Start () {
        ErrorMessage.SetActive(false);

        //googlePlayDebug.GetComponent<TextMesh>().text = "authenticating on google play";
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {            
            //googlePlayDebug.GetComponent<TextMesh>().text = "User authenticate google play games: " + success;
            if (!success) {
                Debug.Log ("Not authenticated");
                ErrorMessage.SetActive(true);
            }
        });	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
