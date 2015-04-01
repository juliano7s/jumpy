using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int score = 0;                   // The player's score.
    public int bestComboCount = 0;
    public int comboCount = 0;
    private int previousScore = -1;          // The score in the previous frame.

    public float castle2Time = 0;
    public float castle3Time = 0;
    public float castle4Time = 0;
    public float castle5Time = 0;
    public float castle6Time = 0;

    private bool sentScore10 = false;
    private bool sentScore50 = false;
    private bool sentScore100 = false;
    private bool sentScore150 = false;

    private bool sentCombo2 = false;
    private bool sentCombo5 = false;
    private bool sentCombo10 = false;
    private bool sentCombo15 = false;
    private bool sentCombo20 = false;

    void Start()
    {

    }

    void Update ()
    {
        if (previousScore != score)
        {
            
            GetComponent<GUIText>().text = "" + score;
            GameObject.Find("score/scoreShadow").GetComponent<GUIText>().text = "" + score;
            previousScore = score;
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
        }

        if (comboCount > 0)
        {
            //GameObject.Find("score/comboMultiplier").GetComponent<GUIText>().text = "+" + comboCount + " combo!";
            //GameObject.Find("score/comboMultiplier/comboShadow").GetComponent<GUIText>().text = "+" + comboCount + " combo!";
            if (bestComboCount < comboCount)
            {
                bestComboCount = comboCount;
                if (bestComboCount >= 2  && !sentCombo2) { 
                    Social.ReportProgress("CgkI5dWk2_MQEAIQEw", 100.0f, (bool success) => {});
                    sentCombo2 = true;
                }
                if (bestComboCount >= 5  && !sentCombo5) { 
                    Social.ReportProgress("CgkI5dWk2_MQEAIQFA", 100.0f, (bool success) => {});
                    sentCombo5 = true;
                }
                if (bestComboCount >= 10 && !sentCombo10) { 
                    Social.ReportProgress("CgkI5dWk2_MQEAIQFQ", 100.0f, (bool success) => {});
                    sentCombo10 = true;
                }
                if (bestComboCount >= 15 && !sentCombo15) { 
                    Social.ReportProgress("CgkI5dWk2_MQEAIQFg", 100.0f, (bool success) => {});
                    sentCombo15 = true;
                }
                if (bestComboCount >= 20 && !sentCombo20) {
                    Social.ReportProgress("CgkI5dWk2_MQEAIQFw", 100.0f, (bool success) => {});
                    sentCombo20 = true;
                }
            }
        }
    }

}
