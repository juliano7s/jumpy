using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{

    public Transform player;
    private JumpyController jumpyController;
    public float platformDeathXTreshold = 40f;

    private Transform childComboAlert;
    private Score scoreScriptObject;
    private bool hasScored;

    public GameObject simplePointPrefab;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        jumpyController = player.GetComponent<JumpyController>();
        scoreScriptObject = GameObject.Find("score").GetComponent<Score>();
        childComboAlert = transform.GetChild(0);
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (player != null)
        {
            float leftXCameraBoundary = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width/Screen.height;
            //Debug.Log ("xCameraBoundary: " + xCameraBoundary);
            //Debug.Log ("lastSpawnedPlatform.transform.position.x - xCameraBoundary =  " + lastSpawnedPlatform.transform.position.x + " - " + xCameraBoundary + " = " + currentXThreshold);
            if ((transform.position.x + ((BoxCollider2D) collider2D).size.x) < leftXCameraBoundary)
            {
                Debug.Log("Player far away. Destroying myself: " + (player.transform.position.x - transform.position.x));
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        // Increases score
        if (collision.gameObject.tag == "Player" && !hasScored) {
            scoreScriptObject.score++;
            Instantiate(simplePointPrefab, collision.transform.position, Quaternion.identity);
            if (jumpyController.isComboJump)
            {
                Debug.Log("Combo jump!!!");
                childComboAlert.audio.Play();
                scoreScriptObject.comboCount++;
                scoreScriptObject.score++;
                Instantiate(simplePointPrefab, collision.transform.position, Quaternion.identity);
            } else {
                audio.Play(); //http://www.bfxr.net/
                scoreScriptObject.score += scoreScriptObject.comboCount;
                scoreScriptObject.comboCount = 0;
            }

            hasScored = true;
        }
    }
}