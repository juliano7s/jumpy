using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{

    public Transform player;
    public float platformDeathXTreshold = 150f;

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
            if ((player.transform.position.x - transform.position.x) > platformDeathXTreshold)
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