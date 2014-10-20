using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int score = -1;                   // The player's score.
    public int comboCount = 0;
    private int previousScore = -1;          // The score in the previous frame.

    void Update ()
    {
        if (previousScore != score)
        {
            Debug.Log ("Score changed to: " + score);
            guiText.text = "" + score;
            previousScore = score;
        }
    }

}
