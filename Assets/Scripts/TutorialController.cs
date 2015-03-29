using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

    public GameObject JumpyObject;
    public Animator TutorialAnimator;

    private float startTime;
    private bool showFirstTutorial = true;
    public bool ShowFirstJump = false;
    private Vector2 velocityWhenPaused;
    private float firstJumpTime = 0.0f;
    public bool ShowCombo = false;


	// Use this for initialization
	void Start () {
        Debug.Log("Starting tutorial controller object");
        JumpyObject.GetComponent<JumpyController>().ShowingTutorial = true;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Floor(Time.time - startTime).Equals(2.0f) && showFirstTutorial) {
            TutorialAnimator.enabled = true;
            showFirstTutorial = false;
        }

        if (ShowFirstJump) {
            Debug.Log("Adding jump");
            JumpyObject.SendMessage("AddJump", new Vector2(2000,3000));
            ShowFirstJump = false;
            firstJumpTime = Time.time;
        }

        if (Mathf.Floor(Time.time - firstJumpTime).Equals(1.0f) && !firstJumpTime.Equals(0)) {

            if (!JumpyObject.GetComponent<Rigidbody2D>().isKinematic) {
                velocityWhenPaused = JumpyObject.GetComponent<Rigidbody2D>().velocity;
                Debug.Log("Velocity: " + velocityWhenPaused);
            }
            JumpyObject.GetComponent<Rigidbody2D>().isKinematic = true;

            TutorialAnimator.SetBool("showComboTutorial", true);
        }

        if (ShowCombo) {
            Debug.Log("Adding jump combo, velocity: " + velocityWhenPaused);
            JumpyObject.GetComponent<Rigidbody2D>().velocity = velocityWhenPaused;
            JumpyObject.GetComponent<Rigidbody2D>().isKinematic = false;
            JumpyObject.SendMessage("AddJump", new Vector2(2000,2000));

            ShowCombo = false;
        }

        if (Mathf.Floor(Time.time - startTime).Equals(12.0f)) {
            Application.LoadLevel("Main");
        }
	}
}
