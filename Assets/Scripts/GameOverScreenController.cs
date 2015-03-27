using UnityEngine;
using GooglePlayGames;
using System.Collections;
using GoogleMobileAds.Api;

public class GameOverScreenController : MonoBehaviour
{

    private Score scoreScriptObject;

    public GameObject ScoreText;
    public GameObject ComboText;
    public GameObject ComboSign;
    public GameObject BestScoreText;
    public GameObject BestComboText;
    public GameObject Castle2Time;
    public GameObject Castle3Time;
    public GameObject Castle4Time;
    public GameObject Castle5Time;
    public GameObject Castle6Time;
    public GameObject BestCastle2Time;
    public GameObject BestCastle3Time;
    public GameObject BestCastle4Time;
    public GameObject BestCastle5Time;
    public GameObject BestCastle6Time;

    private float pointsTimer = 0.1f;
    private float castleTimesTimer = 0.7f;

    private int score = 0;
    private AudioSource scorePointAudio;
    private int combo = 0;
    private AudioSource comboPointAudio;
    public GameObject SimplePointPrefab;

    private int bestScore;
    private int bestCombo;
    private float bestCastle2Time;
    private float bestCastle3Time;
    private float bestCastle4Time;
    private float bestCastle5Time;
    private float bestCastle6Time;
    private bool scoresSent = false;

    private BannerView bannerView;

    // Use this for initialization
    void Start ()
    {

        scoreScriptObject = GameObject.Find ("/score").GetComponent<Score> ();
        scorePointAudio = GameObject.Find ("/gameOverScreen/scoreSign").GetComponent<AudioSource> ();
        comboPointAudio = GameObject.Find ("/gameOverScreen/comboSign").GetComponent<AudioSource> ();
        score = scoreScriptObject.score;
        bestScore = PlayerPrefs.GetInt ("BestScore");
        bestCombo = PlayerPrefs.GetInt ("BestCombo");

        bestCastle2Time = PlayerPrefs.GetFloat ("BestCastle2Time");
        string displaySeconds = (bestCastle2Time % 60).ToString ("00");
        string displayMinutes = Mathf.Floor (bestCastle2Time / 60).ToString ("00"); 
        
        BestCastle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle3Time = PlayerPrefs.GetFloat ("BestCastle3Time");
        displaySeconds = (bestCastle3Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle3Time / 60).ToString ("00"); 
        
        BestCastle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle4Time = PlayerPrefs.GetFloat ("BestCastle4Time");
        displaySeconds = (bestCastle4Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle4Time / 60).ToString ("00"); 
        
        BestCastle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle5Time = PlayerPrefs.GetFloat ("BestCastle5Time");
        displaySeconds = (bestCastle5Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle5Time / 60).ToString ("00"); 
        
        BestCastle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle6Time = PlayerPrefs.GetFloat ("BestCastle6Time");
        displaySeconds = (bestCastle6Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle6Time / 60).ToString ("00"); 
        
        BestCastle6Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        //scoreValue.guiText.text = scoreScriptObject.score.ToString();
        //comboValue.guiText.text = scoreScriptObject.bestComboCount.ToString();
        BestScoreText.GetComponent<TextMesh> ().text = bestScore.ToString ();
        BestComboText.GetComponent<TextMesh> ().text = bestCombo.ToString ();

        Castle2Time.SetActive(false);
        Castle3Time.SetActive(false);
        Castle4Time.SetActive(false);
        Castle5Time.SetActive(false);
        Castle6Time.SetActive(false);

        if (bestCastle2Time <= 0)
            BestCastle2Time.SetActive(false);
        if (bestCastle3Time <= 0)
            BestCastle3Time.SetActive(false);
        if (bestCastle4Time <= 0)
            BestCastle4Time.SetActive(false);
        if (bestCastle5Time <= 0)
            BestCastle5Time.SetActive(false);
        if (bestCastle6Time <= 0)
            BestCastle6Time.SetActive(false);

        gameObject.SetActive (false);
    }

    void OnEnable ()
    {
        Debug.Log("Enabling game over");
        transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        RequestBanner();
        bannerView.Show();
        scoresSent = false;
    }

    void OnDisable ()
    {
        Debug.Log("Disabling game over");
        bannerView.Hide();
    }

    void OnDestroy()
    {
        bannerView.Destroy();
        bannerView = null;
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameObject.activeSelf) {
            pointsTimer -= Time.deltaTime;

            if (!scoresSent) {
                Debug.Log("Sending scores to google play");
                Social.ReportScore(scoreScriptObject.score, "CgkI5dWk2_MQEAIQAA", (bool success) => {});
                Social.ReportScore(scoreScriptObject.bestComboCount, "CgkI5dWk2_MQEAIQBw", (bool success) => {});
                if (scoreScriptObject.castle2Time > 0) {
                    Social.ReportScore(Mathf.FloorToInt(scoreScriptObject.castle2Time * 1000), "CgkI5dWk2_MQEAIQAg", (bool success) => {});
                }
                if (scoreScriptObject.castle3Time > 0) {
                    Social.ReportScore(Mathf.FloorToInt(scoreScriptObject.castle3Time * 1000), "CgkI5dWk2_MQEAIQAw", (bool success) => {});
                }
                if (scoreScriptObject.castle4Time > 0) {
                    Social.ReportScore(Mathf.FloorToInt(scoreScriptObject.castle4Time * 1000), "CgkI5dWk2_MQEAIQBA", (bool success) => {});
                }
                if (scoreScriptObject.castle5Time > 0) {
                    Social.ReportScore(Mathf.FloorToInt(scoreScriptObject.castle5Time * 1000), "CgkI5dWk2_MQEAIQBQ", (bool success) => {});
                }
                if (scoreScriptObject.castle6Time > 0) {
                    Social.ReportScore(Mathf.FloorToInt(scoreScriptObject.castle6Time * 1000), "CgkI5dWk2_MQEAIQBg", (bool success) => {});
                }

                scoresSent = true;
            }

            if (score < scoreScriptObject.score) {
                StartCoroutine ("CountScore");
            }

            if (combo < scoreScriptObject.bestComboCount) {
                StartCoroutine ("CountCombo");
            }

            if (score == scoreScriptObject.score && combo == scoreScriptObject.bestComboCount) {
                castleTimesTimer -= Time.deltaTime;
            }

            CountTimes ();

            PlayerPrefs.Save ();
        }
    }

    IEnumerator CountScore ()
    {
        while (score < scoreScriptObject.score && pointsTimer <= 0) {
            //Vector3 pointPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            Vector3 pointPosition = new Vector3 (ScoreText.transform.position.x, ScoreText.transform.position.y, transform.position.z);
            Instantiate (SimplePointPrefab, pointPosition, Quaternion.identity);
            scorePointAudio.Play ();
            score++;
            ScoreText.GetComponent<TextMesh> ().text = score.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (score > bestScore && score == scoreScriptObject.score && int.Parse (BestScoreText.GetComponent<TextMesh> ().text) < score) {
            Debug.Log ("setting score to best");
            BestScoreText.GetComponent<TextMesh> ().text = score.ToString ();
            PlayerPrefs.SetInt ("BestScore", Mathf.Max (score, bestScore));
        }
    }

    IEnumerator CountCombo ()
    {
        while (combo < scoreScriptObject.bestComboCount && pointsTimer <= 0) {
            //Vector3 pointPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            //Vector3 pointPosition = new Vector3 (ComboText.transform.position.x, ComboText.transform.position.y, transform.position.z);
            //Instantiate (SimplePointPrefab, pointPosition, Quaternion.identity);
            ComboSign.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            comboPointAudio.Play ();
            Debug.Log("Counting combos: " + combo);
            combo++;
            ComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (combo > bestCombo && combo == scoreScriptObject.comboCount) {
            Debug.Log ("setting combo to best");
            BestComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            PlayerPrefs.SetInt ("BestCombo", Mathf.Max (combo, bestCombo));
        }
    }

    void CountTimes ()
    {
        while (castleTimesTimer <= 0) {
            if (scoreScriptObject.castle2Time > 0 && Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Castle2Time.SetActive(true);
                string displaySeconds = (scoreScriptObject.castle2Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle2Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle2Time.GetComponent<AudioSource> ().Play ();

                if (scoreScriptObject.castle2Time < bestCastle2Time || bestCastle2Time <= 0) {
                    Debug.Log ("setting castle 2 time to best");
                    BestCastle2Time.SetActive(true);
                    BestCastle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle2Time", scoreScriptObject.castle2Time);
                }
            } else if (scoreScriptObject.castle3Time > 0 && !Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") &&
                    Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Castle3Time.SetActive(true);
                string displaySeconds = (scoreScriptObject.castle3Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle3Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle3Time.GetComponent<AudioSource> ().Play ();
                
                if (scoreScriptObject.castle3Time < bestCastle3Time || bestCastle3Time <= 0) {
                    Debug.Log ("setting castle 3 time to best");
                    BestCastle3Time.SetActive(true);
                    BestCastle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle3Time", scoreScriptObject.castle3Time);
                }
            } else if (scoreScriptObject.castle4Time > 0 && !Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") &&
                    Castle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Castle4Time.SetActive(true);
                string displaySeconds = (scoreScriptObject.castle4Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle4Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle4Time.GetComponent<AudioSource> ().Play ();
                
                if (scoreScriptObject.castle4Time < bestCastle4Time || bestCastle4Time <= 0) {
                    Debug.Log ("setting castle 4 time to best");
                    BestCastle4Time.SetActive(true);
                    BestCastle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle4Time", scoreScriptObject.castle4Time);
                }
            } else if (scoreScriptObject.castle5Time > 0 && !Castle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") &&
                       Castle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Castle5Time.SetActive(true);
                string displaySeconds = (scoreScriptObject.castle5Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle5Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle5Time.GetComponent<AudioSource> ().Play ();
                
                if (scoreScriptObject.castle5Time < bestCastle5Time || bestCastle5Time <= 0) {
                    Debug.Log ("setting castle 5 time to best");
                    BestCastle5Time.SetActive(true);
                    BestCastle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle5Time", scoreScriptObject.castle5Time);
                }
            } else if (scoreScriptObject.castle6Time > 0 && !Castle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") &&
                       Castle6Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Castle6Time.SetActive(true);
                string displaySeconds = (scoreScriptObject.castle6Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle6Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle6Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle6Time.GetComponent<AudioSource> ().Play ();
                
                if (scoreScriptObject.castle6Time < bestCastle6Time || bestCastle6Time <= 0) {
                    Debug.Log ("setting castle 6 time to best");
                    BestCastle6Time.SetActive(true);
                    BestCastle6Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle6Time", scoreScriptObject.castle6Time);
                }
            }

            castleTimesTimer = 0.7f;
        }
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4495286146662458/1010416125";
        #elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        Debug.Log("REQUESTING AD BANNER");
        if (bannerView == null) {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);
            // Create an empty ad request.
            //AdRequest request = new AdRequest.Builder().Build();
            AdRequest request = new AdRequest.Builder ()
                //.AddTestDevice (AdRequest.TestDeviceSimulator)       // Simulator.
                .AddTestDevice ("75ABC2944FDBB5E13BA1723DEBA591F7")  // My Nexus 4 device
                .Build ();
            // Load the banner with the request.
            bannerView.LoadAd (request);
        }
    }

}


