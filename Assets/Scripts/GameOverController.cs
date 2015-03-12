using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour
{

    private Score scoreScriptObject;

    public GameObject scoreValue;
    public GameObject comboValue;
    public GameObject bestScoreValue;
    public GameObject bestComboValue;

    private float pointsTimer = 0.1f;
    private int score = 0;
    private AudioSource scorePointAudio;
    private int combo = 0;
    private AudioSource comboPointAudio;
    public GameObject simplePointPrefab;

    private int bestScore;
    private int bestCombo;

    // Use this for initialization
    void Start ()
    {

        scoreScriptObject = GameObject.Find("/score").GetComponent<Score>();
        scorePointAudio = GameObject.Find("/gameOver/score").GetComponent<AudioSource>();
        comboPointAudio = GameObject.Find("/gameOver/combo").GetComponent<AudioSource>();
        score = scoreScriptObject.score;
        bestScore = PlayerPrefs.GetInt("BestScore");
        bestCombo = PlayerPrefs.GetInt("BestCombo");
        //scoreValue.guiText.text = scoreScriptObject.score.ToString();
        //comboValue.guiText.text = scoreScriptObject.bestComboCount.ToString();
        bestScoreValue.GetComponent<GUIText>().text = bestScore.ToString();
        bestComboValue.GetComponent<GUIText>().text = bestCombo.ToString();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameObject.activeSelf)
        {
            pointsTimer -= Time.deltaTime;

            StartCoroutine("CountScore");
            StartCoroutine("CountCombo");

            PlayerPrefs.Save();
        }
    }

    IEnumerator CountScore()
    {
        while (score < scoreScriptObject.score && pointsTimer <= 0)
        {
            Vector3 pointPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            Instantiate(simplePointPrefab, pointPosition, Quaternion.identity);
            scorePointAudio.Play();
            score++;
            scoreValue.GetComponent<GUIText>().text = score.ToString();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (score > bestScore && score == scoreScriptObject.score && int.Parse(bestScoreValue.GetComponent<GUIText>().text) < score)
        {
            Debug.Log("setting score to best");
            bestScoreValue.GetComponent<GUIText>().text = score.ToString();
            PlayerPrefs.SetInt("BestScore", Mathf.Max (score, bestScore));
        }
    }

    IEnumerator CountCombo()
    {
        while (combo < scoreScriptObject.bestComboCount && pointsTimer <= 0)
        {
            Vector3 pointPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            Instantiate(simplePointPrefab, pointPosition, Quaternion.identity);
            comboPointAudio.Play();
            combo++;
            comboValue.GetComponent<GUIText>().text = combo.ToString();
            pointsTimer = 0.1f;
            yield return null;
        }

        if (combo > bestCombo && combo == scoreScriptObject.comboCount  && int.Parse(bestComboValue.GetComponent<GUIText>().text) < combo)
        {
            Debug.Log("setting combo to best");
            bestComboValue.GetComponent<GUIText>().text = combo.ToString();
            PlayerPrefs.SetInt("BestCombo", Mathf.Max (combo, bestCombo));
        }
    }

}

