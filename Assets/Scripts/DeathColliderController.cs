using UnityEngine;
using System.Collections;

public class DeathColliderController : MonoBehaviour
{
    public Transform player;
    public GameObject gameOverScreen;
    public GameObject score;
	public GameObject timer;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // If the player hits the trigger...
        if(col.gameObject.tag == "Player")
        {
            // .. stop the camera tracking the player
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
            
            // ... destroy the player.
            Destroy (col.gameObject);
            // ... reload the level.
            StartCoroutine("GameOver");
        }

        if (col.gameObject.tag == "Point")
        {
            Destroy(col.gameObject);    // Not ideal instatiating and destroying everytime
            //Debug.Log("Destroying point");
        }
    }

    IEnumerator GameOver()
    {
        // ... pause briefly
        yield return new WaitForSeconds(1);
        score.GetComponent<GUIText>().enabled = false;
        score.transform.GetChild(0).GetComponent<GUIText>().enabled = false;
		timer.GetComponent<GUIText>().enabled = false;
        timer.transform.GetChild(0).GetComponent<GUIText>().enabled = false;
        //gameOver.SetActive(true);
        gameOverScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        if (player != null)
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}