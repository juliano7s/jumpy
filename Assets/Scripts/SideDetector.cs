using UnityEngine;
using System.Collections;

public class SideDetector : MonoBehaviour
{
    private Movement mScript;
        
    // Use this for initialization
    void Start ()
    {
        mScript = transform.parent.GetComponent<Movement> ();
    }
    
    // Update is called once per frame
    void Update ()
    {        
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);
        
        if (hit.collider != null && hit.collider == this.collider2D) {
            //Direction of movement is opposite of collider detection
            float direction = Mathf.Sign(collider2D.transform.position.x - transform.parent.position.x);
            mScript.goRight = -direction;
            //Debug.Log ("Mouse is over " + name + " - direction is " + direction);
        }
    }
    
}
