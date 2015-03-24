using UnityEngine;
using System.Collections;

public class CastleController : MonoBehaviour {

    public GameObject CastleSignPrefab;
    private GameObject castleSign;
    private Vector3 signPosition;
    private float signTimer = 5.0f;

	private Timer timerScriptObject;
    private Score scoreScriptObject;

    private bool hasScored = false;

	// Use this for initialization
	void Start () {
        scoreScriptObject = GameObject.Find ("score").GetComponent<Score> ();
        timerScriptObject = GameObject.Find ("timer").GetComponent<Timer> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (castleSign != null)
        {
            signTimer -= Time.deltaTime;
            float cameraLeft = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
            float cameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;
            signPosition = new Vector3 (cameraLeft + 10, cameraTop - 10, transform.position.z);
            castleSign.transform.position = signPosition;

            if (signTimer <= 0) {
                Debug.Log ("Destroying castle sign.");
                signTimer = 5.0f;
                Destroy (castleSign);
            }
        }
	}

    void OnCollisionEnter2D (Collision2D collision)
    {
        // Increases score
        if (collision.gameObject.tag == "Player") {             
            if (GetComponent<BoxCollider2D>().IsTouching(collision.collider) && !hasScored) {
                GetComponent<AudioSource> ().Play ();
                if (transform.name.Equals("castle2")) {
                    scoreScriptObject.castle2Time = timerScriptObject.currentTime;
                    InstantiateCastleSign(scoreScriptObject.castle2Time);
                    hasScored = true;
                }

                if (transform.name.Equals("castle3")) {
                    scoreScriptObject.castle3Time = timerScriptObject.currentTime;
                    InstantiateCastleSign(scoreScriptObject.castle3Time);
                    hasScored = true;
                }

                if (transform.name.Equals("castle4")) {
                    scoreScriptObject.castle4Time = timerScriptObject.currentTime;
                    InstantiateCastleSign(scoreScriptObject.castle4Time);
                    hasScored = true;
                }

                if (transform.name.Equals("castle5")) {
                    scoreScriptObject.castle5Time = timerScriptObject.currentTime;
                    InstantiateCastleSign(scoreScriptObject.castle5Time);
                    hasScored = true;
                }

                if (transform.name.Equals("castle6")) {
                    scoreScriptObject.castle6Time = timerScriptObject.currentTime;
                    InstantiateCastleSign(scoreScriptObject.castle6Time);
                    hasScored = true;
                }
            } 
        }
    }

    void InstantiateCastleSign(float currentTime)
    {
        Debug.Log ("Instantiating castle time");
        if (castleSign == null) {        
            castleSign = (GameObject) Instantiate(CastleSignPrefab, signPosition, Quaternion.identity);
        }
        string displaySeconds = (currentTime % 60).ToString("00");
        string displayMinutes = Mathf.Floor(currentTime / 60).ToString("00"); 
        
        string text = displayMinutes + ":" + displaySeconds; 
        castleSign.transform.GetChild(0).GetComponent<TextMesh>().text = text;
    }
}
