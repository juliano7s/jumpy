using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        /* ignore collisions between lings */
        object[] obj = GameObject.FindObjectsOfType (typeof(GameObject));
        foreach (object o in obj) {
            GameObject g = (GameObject)o;
            if (g.tag == "Ling") {
                foreach (object u in obj) {
                    GameObject h = (GameObject)u;
                    if (h.tag == "Ling")
                        Physics2D.IgnoreCollision (g.collider2D, h.collider2D);
                }
            }    
        }   
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
}
