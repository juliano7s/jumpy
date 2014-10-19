using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{

    public Transform player;
    public float platformDeathXTreshold = 40f;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        // Change direction when hit Wall
        if (collision.gameObject.tag == "Wall") {
            return;
        }
    }
}