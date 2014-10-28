using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int score = 0;                   // The player's score.
    public int comboCount = 0;
    private int previousScore = -1;          // The score in the previous frame.

    void Update ()
    {
        if (previousScore != score)
        {
            Debug.Log ("Score changed to: " + score);
            guiText.text = "" + score;
            GameObject.Find("score/scoreShadow").guiText.text = "" + score;
            previousScore = score;
        }

        if (comboCount > 0)
        {
            GameObject.Find("score/comboMultiplier").guiText.text = "+" + comboCount + "!";
            GameObject.Find("score/comboMultiplier/comboShadow").guiText.text = "+" + comboCount + "!";
        } else
        {
            GameObject.Find("score/comboMultiplier").guiText.text = "";
            GameObject.Find("score/comboMultiplier/comboShadow").guiText.text = "";
        }
    }

}
