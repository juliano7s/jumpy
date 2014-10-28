using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {

    public float xForce, yForce;

	// Use this for initialization
	void Start () {

    }

    void Awake() {
        xForce = Random.Range(xForce, 5 * xForce);
        rigidbody2D.AddForce(new Vector2(xForce, yForce));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
