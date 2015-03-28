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
    public GameObject BestScore;
    public GameObject BestCombo;
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

        BestScore.transform.GetChild(0).GetComponent<TextMesh> ().text = bestScore.ToString ();
        BestCombo.transform.GetChild(0).GetComponent<TextMesh> ().text = bestCombo.ToString ();

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
        PlayerPrefs.SetInt("BestScore", Mathf.Max(scoreScriptObject.score, bestScore));
        PlayerPrefs.SetInt("BestCombo", Mathf.Max(scoreScriptObject.bestComboCount, bestCombo));

        setBestCastleTimes();

        transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        RequestBanner();
        bannerView.Show();
    }

    void OnDisable ()
    {
        Debug.Log("Disabling game over");       
        Social.ReportScore(PlayerPrefs.GetInt("BestScore"), "CgkI5dWk2_MQEAIQDg", (bool success) => {});
        Social.ReportScore(PlayerPrefs.GetInt("BestCombo"), "CgkI5dWk2_MQEAIQBw", (bool success) => {});

        if (PlayerPrefs.GetFloat("BestCastle2Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(PlayerPrefs.GetFloat("BestCastle2Time") * 1000), "CgkI5dWk2_MQEAIQAg", (bool success) => {});
        }
        if (PlayerPrefs.GetFloat("BestCastle3Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(PlayerPrefs.GetFloat("BestCastle3Time") * 1000), "CgkI5dWk2_MQEAIQAw", (bool success) => {});
        }
        if (PlayerPrefs.GetFloat("BestCastle4Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(PlayerPrefs.GetFloat("BestCastle4Time") * 1000), "CgkI5dWk2_MQEAIQBA", (bool success) => {});
        }
        if (PlayerPrefs.GetFloat("BestCastle5Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(PlayerPrefs.GetFloat("BestCastle5Time") * 1000), "CgkI5dWk2_MQEAIQBQ", (bool success) => {});
        }
        if (PlayerPrefs.GetFloat("BestCastle6Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(PlayerPrefs.GetFloat("BestCastle6Time") * 1000), "CgkI5dWk2_MQEAIQBg", (bool success) => {});
        }

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

        if (score > bestScore && score == scoreScriptObject.score && int.Parse (BestScore.transform.GetChild(0).GetComponent<TextMesh> ().text) < score) {
            Debug.Log ("setting score to best");
            BestScore.transform.GetChild(0).GetComponent<TextMesh> ().text = score.ToString ();
            PlayerPrefs.SetInt ("BestScore", score);
            bestScore = score;
        }
    }

    IEnumerator CountCombo ()
    {
        while (combo < scoreScriptObject.bestComboCount && pointsTimer <= 0) {
            ComboSign.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            comboPointAudio.Play ();
            Debug.Log("Counting combos: " + combo);
            combo++;
            ComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (combo > bestCombo && combo == scoreScriptObject.bestComboCount) {
            Debug.Log ("setting combo to best");
            BestCombo.transform.GetChild(0).GetComponent<TextMesh> ().text = combo.ToString ();
            PlayerPrefs.SetInt ("BestCombo", combo);
            bestCombo = combo;
        }
    }

    void CountTimes ()
    {
        while (castleTimesTimer <= 0) {
            if (scoreScriptObject.castle2Time > 0 && Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {

                processCastleTime (Castle2Time, BestCastle2Time, scoreScriptObject.castle2Time, "BestCastle2Time");

            } else if (scoreScriptObject.castle3Time > 0 &&
                (!Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") || (scoreScriptObject.castle2Time <= 0)) &&
                Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {

                processCastleTime (Castle3Time, BestCastle3Time, scoreScriptObject.castle3Time, "BestCastle3Time");

            } else if (scoreScriptObject.castle4Time > 0 &&
                (!Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") || (scoreScriptObject.castle3Time <= 0)) &&
                Castle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {

                processCastleTime (Castle4Time, BestCastle4Time, scoreScriptObject.castle4Time, "BestCastle4Time");

            } else if (scoreScriptObject.castle5Time > 0 &&
                (!Castle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") || (scoreScriptObject.castle4Time <= 0)) &&
                Castle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {

                processCastleTime (Castle5Time, BestCastle5Time, scoreScriptObject.castle5Time, "BestCastle5Time");

            } else if (scoreScriptObject.castle6Time > 0 &&
                (!Castle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") || (scoreScriptObject.castle5Time <= 0)) &&
                Castle6Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {

                processCastleTime (Castle6Time, BestCastle6Time, scoreScriptObject.castle6Time, "BestCastle6Time");
            }

            castleTimesTimer = 0.7f;
        }
    }

    private void processCastleTime(GameObject castleTimeSign, GameObject bestCastleTimeSign, float castleTime, string castleTimePref)
    {
        castleTimeSign.SetActive(true);
        string displaySeconds = (castleTime % 60).ToString ("00");
        string displayMinutes = Mathf.Floor (castleTime / 60).ToString ("00"); 
        
        string text = displayMinutes + ":" + displaySeconds;
        castleTimeSign.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
        castleTimeSign.GetComponent<AudioSource> ().Play ();

        float bestCastleTime = PlayerPrefs.GetFloat(castleTimePref);
        if (castleTime < bestCastleTime || bestCastleTime <= 0) {
            Debug.Log ("setting " + castleTimePref + " to best");
            bestCastleTimeSign.SetActive(true);
            bestCastleTimeSign.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
            PlayerPrefs.SetFloat (castleTimePref, castleTime);
        }
    }

    private void setBestCastleTimes()
    {
        if (scoreScriptObject.castle2Time > 0) {
            if (PlayerPrefs.GetFloat("BestCastle2Time") > 0) {
                PlayerPrefs.SetFloat("BestCastle2Time", Mathf.Min(scoreScriptObject.castle2Time, PlayerPrefs.GetFloat("BestCastle2Time")));
            } else {
                PlayerPrefs.SetFloat("BestCastle2Time", scoreScriptObject.castle2Time);
            }
        }

        if (scoreScriptObject.castle3Time > 0) {
            if (PlayerPrefs.GetFloat("BestCastle3Time") > 0) {
                PlayerPrefs.SetFloat("BestCastle3Time", Mathf.Min(scoreScriptObject.castle3Time, PlayerPrefs.GetFloat("BestCastle3Time")));
            } else {
                PlayerPrefs.SetFloat("BestCastle3Time", scoreScriptObject.castle3Time);
            }
        }

        if (scoreScriptObject.castle4Time > 0) {
            if (PlayerPrefs.GetFloat("BestCastle4Time") > 0) {
                PlayerPrefs.SetFloat("BestCastle4Time", Mathf.Min(scoreScriptObject.castle4Time, PlayerPrefs.GetFloat("BestCastle4Time")));
            } else {
                PlayerPrefs.SetFloat("BestCastle4Time", scoreScriptObject.castle4Time);
            }
        }

        if (scoreScriptObject.castle5Time > 0) {
            if (PlayerPrefs.GetFloat("BestCastle5Time") > 0) {
                PlayerPrefs.SetFloat("BestCastle5Time", Mathf.Min(scoreScriptObject.castle5Time, PlayerPrefs.GetFloat("BestCastle5Time")));
            } else {
                PlayerPrefs.SetFloat("BestCastle5Time", scoreScriptObject.castle5Time);
            }
        }

        if (scoreScriptObject.castle6Time > 0) {
            if (PlayerPrefs.GetFloat("BestCastle6Time") > 0) {
                PlayerPrefs.SetFloat("BestCastle6Time", Mathf.Min(scoreScriptObject.castle6Time, PlayerPrefs.GetFloat("BestCastle6Time")));
            } else {
                PlayerPrefs.SetFloat("BestCastle6Time", scoreScriptObject.castle6Time);
            }
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


