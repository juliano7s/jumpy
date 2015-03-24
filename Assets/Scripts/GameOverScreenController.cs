using UnityEngine;
using System.Collections;

public class GameOverScreenController : MonoBehaviour
{

    private Score scoreScriptObject;

    public GameObject ScoreText;
    public GameObject ComboText;
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
        gameObject.SetActive (false);
    }

    void OnEnable ()
    {
        transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameObject.activeSelf) {
            pointsTimer -= Time.deltaTime;

            StartCoroutine ("CountScore");
            StartCoroutine ("CountCombo");

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
            Vector3 pointPosition = new Vector3 (ComboText.transform.position.x, ComboText.transform.position.y, transform.position.z);
            Instantiate (SimplePointPrefab, pointPosition, Quaternion.identity);
            comboPointAudio.Play ();
            combo++;
            ComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (combo > bestCombo && combo == scoreScriptObject.comboCount && int.Parse (BestComboText.GetComponent<TextMesh> ().text) < combo) {
            Debug.Log ("setting combo to best");
            BestComboText.GetComponent<TextMesh> ().text = combo.ToString ();
            PlayerPrefs.SetInt ("BestCombo", Mathf.Max (combo, bestCombo));
        }
    }

    void CountTimes ()
    {
        while (castleTimesTimer <= 0) {
            if (scoreScriptObject.castle2Time > 0 && Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                Debug.Log ("castle 2 time > 0");
                string displaySeconds = (scoreScriptObject.castle2Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle2Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle2Time.GetComponent<AudioSource> ().Play ();

                if (scoreScriptObject.castle2Time < bestCastle2Time || bestCastle2Time <= 0) {
                    Debug.Log ("setting castle 2 time to best");
                    BestCastle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle2Time", scoreScriptObject.castle2Time);
                }
            }

            if (scoreScriptObject.castle3Time > 0 && !Castle2Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00") &&
                Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text.Equals ("00:00")) {
                string displaySeconds = (scoreScriptObject.castle3Time % 60).ToString ("00");
                string displayMinutes = Mathf.Floor (scoreScriptObject.castle3Time / 60).ToString ("00"); 
                
                string text = displayMinutes + ":" + displaySeconds;
                Castle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                Castle3Time.GetComponent<AudioSource> ().Play ();
                
                if (scoreScriptObject.castle3Time > bestCastle3Time || bestCastle3Time <= 0) {
                    Debug.Log ("setting castle 3 time to best");
                    BestCastle3Time.transform.GetChild (0).GetComponent<TextMesh> ().text = text;
                    PlayerPrefs.SetFloat ("BestCastle3Time", scoreScriptObject.castle3Time);
                }
            }

            castleTimesTimer = 0.7f;
        }
    }

}


