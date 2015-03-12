using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int score = 0;                   // The player's score.
    public int bestComboCount = 0;
    public int comboCount = 0;
    private int previousScore = -1;          // The score in the previous frame.

    void Update ()
    {
        if (previousScore != score)
        {
            Debug.Log ("Score changed to: " + score);
            GetComponent<GUIText>().text = "" + score;
            GameObject.Find("score/scoreShadow").GetComponent<GUIText>().text = "" + score;
            previousScore = score;
        }

        if (comboCount > 0)
        {
            GameObject.Find("score/comboMultiplier").GetComponent<GUIText>().text = "+" + comboCount + " combo!";
            GameObject.Find("score/comboMultiplier/comboShadow").GetComponent<GUIText>().text = "+" + comboCount + " combo!";
            if (bestComboCount < comboCount)
            {
                bestComboCount = comboCount;
            }
        } else
        {
            GameObject.Find("score/comboMultiplier").GetComponent<GUIText>().text = "";
            GameObject.Find("score/comboMultiplier/comboShadow").GetComponent<GUIText>().text = "";
        }
    }

}
