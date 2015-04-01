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
    private bool initialized = false;
    private bool stopCounting = false;

    private bool sentScore10 = false;
    private bool sentScore50 = false;
    private bool sentScore100 = false;
    private bool sentScore150 = false;
    
    private bool sentCombo2 = false;
    private bool sentCombo5 = false;
    private bool sentCombo10 = false;
    private bool sentCombo15 = false;
    private bool sentCombo20 = false;

    // Use this for initialization
    void Start ()
    {
        
        scoreScriptObject = GameObject.Find ("/score").GetComponent<Score> ();
        scorePointAudio = GameObject.Find ("/gameOverScreen/scoreSign").GetComponent<AudioSource> ();
        comboPointAudio = GameObject.Find ("/gameOverScreen/comboSign").GetComponent<AudioSource> ();
        score = scoreScriptObject.score;
        bestScore = EncryptedPlayerPrefs.GetInt ("BestScore");
        bestCombo = EncryptedPlayerPrefs.GetInt ("BestCombo");

        bestCastle2Time = EncryptedPlayerPrefs.GetFloat ("BestCastle2Time");
        string displaySeconds = (bestCastle2Time % 60).ToString ("00");
        string displayMinutes = Mathf.Floor (bestCastle2Time / 60).ToString ("00"); 
        
        BestCastle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle3Time = EncryptedPlayerPrefs.GetFloat ("BestCastle3Time");
        displaySeconds = (bestCastle3Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle3Time / 60).ToString ("00"); 
        
        BestCastle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle4Time = EncryptedPlayerPrefs.GetFloat ("BestCastle4Time");
        displaySeconds = (bestCastle4Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle4Time / 60).ToString ("00"); 
        
        BestCastle4Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle5Time = EncryptedPlayerPrefs.GetFloat ("BestCastle5Time");
        displaySeconds = (bestCastle5Time % 60).ToString ("00");
        displayMinutes = Mathf.Floor (bestCastle5Time / 60).ToString ("00"); 
        
        BestCastle5Time.transform.GetChild (0).GetComponent<TextMesh> ().text = displayMinutes + ":" + displaySeconds;

        bestCastle6Time = EncryptedPlayerPrefs.GetFloat ("BestCastle6Time");
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
        initialized = true;
    }

    void OnEnable ()
    {
        if (initialized) {
            
            EncryptedPlayerPrefs.SetInt ("BestScore", Mathf.Max (scoreScriptObject.score, bestScore));
            EncryptedPlayerPrefs.SetInt ("BestCombo", Mathf.Max (scoreScriptObject.bestComboCount, bestCombo));

            //Send only the last player score and combo to play services
            Social.ReportScore(scoreScriptObject.score, "CgkI5dWk2_MQEAIQDg", (bool success) => {});
            Social.ReportScore(scoreScriptObject.bestComboCount, "CgkI5dWk2_MQEAIQBw", (bool success) => {});

            setBestCastleTimes ();

            transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            RequestBanner ();
            bannerView.Show ();
        }
    }

    void OnDisable ()
    {
               
        //Social.ReportScore(EncryptedPlayerPrefs.GetInt("BestScore"), "CgkI5dWk2_MQEAIQDg", (bool success) => {});
        //Social.ReportScore(EncryptedPlayerPrefs.GetInt("BestCombo"), "CgkI5dWk2_MQEAIQBw", (bool success) => {});

        if (EncryptedPlayerPrefs.GetFloat("BestCastle2Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(EncryptedPlayerPrefs.GetFloat("BestCastle2Time") * 1000), "CgkI5dWk2_MQEAIQAg", (bool success) => {});
        }
        if (EncryptedPlayerPrefs.GetFloat("BestCastle3Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(EncryptedPlayerPrefs.GetFloat("BestCastle3Time") * 1000), "CgkI5dWk2_MQEAIQAw", (bool success) => {});
        }
        if (EncryptedPlayerPrefs.GetFloat("BestCastle4Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(EncryptedPlayerPrefs.GetFloat("BestCastle4Time") * 1000), "CgkI5dWk2_MQEAIQBA", (bool success) => {});
        }
        if (EncryptedPlayerPrefs.GetFloat("BestCastle5Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(EncryptedPlayerPrefs.GetFloat("BestCastle5Time") * 1000), "CgkI5dWk2_MQEAIQBQ", (bool success) => {});
        }
        if (EncryptedPlayerPrefs.GetFloat("BestCastle6Time") > 0) {
            Social.ReportScore(Mathf.FloorToInt(EncryptedPlayerPrefs.GetFloat("BestCastle6Time") * 1000), "CgkI5dWk2_MQEAIQBg", (bool success) => {});
        }

        //Score achievements retroactively
        if (score >= 10  && !sentScore10) {
            Social.ReportProgress("CgkI5dWk2_MQEAIQDw", 100.0f, (bool success) => {});
            sentScore10 = true;
        }
        if (score >= 50  && !sentScore50){ 
            Social.ReportProgress("CgkI5dWk2_MQEAIQEA", 100.0f, (bool success) => {});
            sentScore50 = true;
        }
        if (score >= 100 && !sentScore100) { 
            Social.ReportProgress("CgkI5dWk2_MQEAIQEQ", 100.0f, (bool success) => {});
            sentScore100 = true;
        }
        if (score >= 150 && !sentScore150) {
            Social.ReportProgress("CgkI5dWk2_MQEAIQEg", 100.0f, (bool success) => {});
            sentScore150 = true;
        }

        //Combo achievements retroactively
        if (scoreScriptObject.bestComboCount >= 2  && !sentCombo2) { 
            Social.ReportProgress("CgkI5dWk2_MQEAIQEw", 100.0f, (bool success) => {});
            sentCombo2 = true;
        }
        if (scoreScriptObject.bestComboCount >= 5  && !sentCombo5) { 
            Social.ReportProgress("CgkI5dWk2_MQEAIQFA", 100.0f, (bool success) => {});
            sentCombo5 = true;
        }
        if (scoreScriptObject.bestComboCount >= 10 && !sentCombo10) { 
            Social.ReportProgress("CgkI5dWk2_MQEAIQFQ", 100.0f, (bool success) => {});
            sentCombo10 = true;
        }
        if (scoreScriptObject.bestComboCount >= 15 && !sentCombo15) { 
            Social.ReportProgress("CgkI5dWk2_MQEAIQFg", 100.0f, (bool success) => {});
            sentCombo15 = true;
        }
        if (scoreScriptObject.bestComboCount >= 20 && !sentCombo20) {
            Social.ReportProgress("CgkI5dWk2_MQEAIQFw", 100.0f, (bool success) => {});
            sentCombo20 = true;
        }
        
        if (bannerView != null)
            bannerView.Hide();
    }
    
    void OnDestroy()
    {
        if (bannerView != null)
            bannerView.Destroy();
        bannerView = null;
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameObject.activeSelf) {
#if UNITY_ANDROID
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
#else
            if (Input.GetMouseButtonUp(0))
#endif
            {
                stopCounting = true;
            }

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
        while (score < scoreScriptObject.score && pointsTimer <= 0 && !stopCounting) {
            //Vector3 pointPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            Vector3 pointPosition = new Vector3 (ScoreText.transform.position.x, ScoreText.transform.position.y, transform.position.z);
            Instantiate (SimplePointPrefab, pointPosition, Quaternion.identity);
            scorePointAudio.Play ();
            score++;
            ScoreText.GetComponent<TextMesh> ().text = score.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (stopCounting) {
            score = scoreScriptObject.score;
            ScoreText.GetComponent<TextMesh> ().text = scoreScriptObject.score.ToString ();
        }

        if (score > bestScore && score == scoreScriptObject.score && int.Parse (BestScore.transform.GetChild(0).GetComponent<TextMesh> ().text) < score) {
            
            BestScore.transform.GetChild(0).GetComponent<TextMesh> ().text = score.ToString ();
            EncryptedPlayerPrefs.SetInt ("BestScore", score);
            bestScore = score;
        }
    }

    IEnumerator CountCombo ()
    {
        while (combo < scoreScriptObject.bestComboCount && pointsTimer <= 0 && !stopCounting) {
            ComboSign.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            comboPointAudio.Play ();
            
            combo++;
            ComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (stopCounting) {
            combo = scoreScriptObject.bestComboCount;
            ComboText.GetComponent<TextMesh> ().text = scoreScriptObject.bestComboCount.ToString();
        }

        if (combo > bestCombo && combo == scoreScriptObject.bestComboCount) {
            
            BestCombo.transform.GetChild(0).GetComponent<TextMesh> ().text = combo.ToString ();
            EncryptedPlayerPrefs.SetInt ("BestCombo", combo);
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

        float bestCastleTime = EncryptedPlayerPrefs.GetFloat(castleTimePref);
        
        
        if (castleTime <= bestCastleTime) {
            
            bestCastleTimeSign.SetActive(true);
            bestCastleTimeSign.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
            EncryptedPlayerPrefs.SetFloat (castleTimePref, castleTime);
        }
    }

    private void setBestCastleTimes()
    {
        if (scoreScriptObject.castle2Time > 0) {
            if (EncryptedPlayerPrefs.GetFloat("BestCastle2Time") > 0) {
                EncryptedPlayerPrefs.SetFloat("BestCastle2Time", Mathf.Min(scoreScriptObject.castle2Time, EncryptedPlayerPrefs.GetFloat("BestCastle2Time")));
            } else {
                EncryptedPlayerPrefs.SetFloat("BestCastle2Time", scoreScriptObject.castle2Time);
            }
            Social.ReportProgress("CgkI5dWk2_MQEAIQCA", 100.0f, (bool success) => {}); //castle achievement
        }

        if (scoreScriptObject.castle3Time > 0) {
            if (EncryptedPlayerPrefs.GetFloat("BestCastle3Time") > 0) {
                EncryptedPlayerPrefs.SetFloat("BestCastle3Time", Mathf.Min(scoreScriptObject.castle3Time, EncryptedPlayerPrefs.GetFloat("BestCastle3Time")));
            } else {
                EncryptedPlayerPrefs.SetFloat("BestCastle3Time", scoreScriptObject.castle3Time);
            }
            Social.ReportProgress("CgkI5dWk2_MQEAIQCQ", 100.0f, (bool success) => {}); //castle achievement
        }

        if (scoreScriptObject.castle4Time > 0) {
            if (EncryptedPlayerPrefs.GetFloat("BestCastle4Time") > 0) {
                EncryptedPlayerPrefs.SetFloat("BestCastle4Time", Mathf.Min(scoreScriptObject.castle4Time, EncryptedPlayerPrefs.GetFloat("BestCastle4Time")));
            } else {
                EncryptedPlayerPrefs.SetFloat("BestCastle4Time", scoreScriptObject.castle4Time);
            }
            Social.ReportProgress("CgkI5dWk2_MQEAIQCg", 100.0f, (bool success) => {}); //castle achievement
        }

        if (scoreScriptObject.castle5Time > 0) {
            if (EncryptedPlayerPrefs.GetFloat("BestCastle5Time") > 0) {
                EncryptedPlayerPrefs.SetFloat("BestCastle5Time", Mathf.Min(scoreScriptObject.castle5Time, EncryptedPlayerPrefs.GetFloat("BestCastle5Time")));
            } else {
                EncryptedPlayerPrefs.SetFloat("BestCastle5Time", scoreScriptObject.castle5Time);
            }
            Social.ReportProgress("CgkI5dWk2_MQEAIQCw", 100.0f, (bool success) => {}); //castle achievement
        }

        if (scoreScriptObject.castle6Time > 0) {
            if (EncryptedPlayerPrefs.GetFloat("BestCastle6Time") > 0) {
                EncryptedPlayerPrefs.SetFloat("BestCastle6Time", Mathf.Min(scoreScriptObject.castle6Time, EncryptedPlayerPrefs.GetFloat("BestCastle6Time")));
            } else {
                EncryptedPlayerPrefs.SetFloat("BestCastle6Time", scoreScriptObject.castle6Time);
            }
            Social.ReportProgress("CgkI5dWk2_MQEAIQDA", 100.0f, (bool success) => {}); //castle achievement
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


