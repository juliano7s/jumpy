using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    public float xJumpForce = 200.0f;
    public float yJumpForce = 300.0f;
    public float jumpRate = 0.7f; //seconds
    public float goRight = 1.0f;
    
    private float lastJumpMove = 0.0f; //seconds

    // Use this for initialization
    void Start ()
    {
        lastJumpMove = Time.time;
    }

    void FixedUpdate ()
    {
        float timeSinceLastJump = Time.time - lastJumpMove;
        if (timeSinceLastJump > jumpRate) {
            rigidbody2D.AddForce (new Vector2 (xJumpForce * goRight, yJumpForce));
            lastJumpMove = Time.time;
            //Debug.Log ("Force added: " + xJumpForce * goRight + "," + yJumpForce);
        }            
    }
    
    void OnCollisionEnter2D (Collision2D collision)
    {
        // Change direction when hit Wall
        if (collision.gameObject.tag == "Wall") {
            goRight = -goRight;
            return;
        }
       
        /* The collision ignoring is made on the Init object
        if (collision.gameObject.tag == "Ling")
        {
            Debug.Log ("Collided with a Ling");
            Physics2D.IgnoreCollision(collision.collider, collider2D);
            return;    
        }
        */       
        
    }
}
