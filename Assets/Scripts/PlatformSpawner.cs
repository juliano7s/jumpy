using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {

    public GameObject platformLargePrefab;
    public GameObject platformMediumPrefab;
    public GameObject platformSmallPrefab;
    private GameObject platformPrefab;

    //private Score scoreScriptObject;

    public Transform player;
    public float newSpawnXThreshold = 100f; //threshold to spawn a new platform in relation to X boundary of the camera
    public float yPositionRangeBottom = 0f, yPositionRangeTop = 30f;
	private float platformDistance = 45.5f;
    private float xCameraBoundary;

    private GameObject lastSpawnedPlatform;
    private Queue platformQueue;
    private int platformCount = 0;
    public GameObject nextPlatformArrow;

	private GameObject lastSpawnedCastle;
	private GameObject castle2;
	private GameObject castle3;
	private GameObject castle4;
	private GameObject castle5;
	private GameObject castle6;


    // Use this for initialization
    void Start () {
        //Debug.Log("Starting PlatformSpawner");
        //scoreScriptObject = GameObject.Find("/score").GetComponent<Score>();

        platformPrefab = platformLargePrefab;

        //Debug.Log("Initializing platformSpawner");
        xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        platformQueue = new Queue();

		float lastSpawnedPosition = player.position.x;
        for (int i = 1; i < 4; i++)
        {
            //Debug.Log("Instantiating platform from prefab: " + platformPrefab);
            //Debug.Log("--- collider scale: " + ((BoxCollider2D) platformPrefab.collider2D).size);
			//Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = i == 1 ? player.position.y - 10 : Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPosition + platformDistance, yRandom);
			lastSpawnedPosition += platformDistance;
            SpawnPlatform(position);
        }
        GameObject nextPlatform = platformQueue.Count > 0 ? (GameObject) platformQueue.Peek() : null;
        nextPlatformArrow.transform.position = new Vector3(xCameraBoundary, nextPlatform.transform.position.y, nextPlatformArrow.transform.position.z);
    }

    void Update() {
        //Debug.Log("Camera position: " + Camera.main.transform.position);
        //Debug.Log("Camera width: " + Camera.main.orthographicSize * Screen.width/Screen.height);
        xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        float currentXThreshold = Mathf.Abs(lastSpawnedPlatform.transform.position.x - xCameraBoundary);
        //Debug.Log ("xCameraBoundary: " + xCameraBoundary);
        //Debug.Log ("lastSpawnedPlatform.transform.position.x - xCameraBoundary =  " + lastSpawnedPlatform.transform.position.x + " - " + xCameraBoundary + " = " + currentXThreshold);
        if (currentXThreshold < newSpawnXThreshold)
        {
            //Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPlatform.transform.position.x + platformDistance, yRandom);
            //Debug.Log("Spawning platform at: " + position);
            SpawnPlatform(position);
        }

        GameObject nextPlatform = platformQueue.Count > 0 ? (GameObject) platformQueue.Peek() : null;
        if ((nextPlatform.transform.position.x - ((BoxCollider2D)nextPlatform.GetComponent<Collider2D>()).size.x / 2) < xCameraBoundary)
        {
            platformQueue.Dequeue();
            nextPlatform = platformQueue.Count > 0 ? (GameObject) platformQueue.Peek() : null;
            nextPlatformArrow.transform.position = new Vector3(xCameraBoundary, nextPlatform.transform.position.y, nextPlatformArrow.transform.position.z);
        } else
        {
            nextPlatformArrow.transform.position = new Vector3(xCameraBoundary, nextPlatformArrow.transform.position.y, nextPlatformArrow.transform.position.z);
        }
    }

    void SpawnPlatform(Vector2 position) {
        
		//Debug.Log("score before spawning: " + scoreScriptObject.score);
		bool castleSpawned = false;

		if (platformCount >= 5 && castle2 == null)
		{			
			castle2 = GameObject.Find("castle2");
			castle2.transform.position = new Vector3(position.x + 5, position.y, castle2.transform.position.z);
			
			lastSpawnedCastle = castle2;
			castleSpawned = true;
		}

        if (platformCount >= 10 && castle3 == null)
		{			
			castle3 = GameObject.Find("castle3");
            castle3.transform.position = new Vector3(position.x + platformDistance / 2, position.y, castle2.transform.position.z);
            platformPrefab = platformMediumPrefab;
            platformDistance = 35.5f;
			lastSpawnedCastle = castle3;
			castleSpawned = true;
		}

        if (platformCount >= 20 && castle4 == null)
		{
			castle4 = GameObject.Find("castle4");
            castle4.transform.position = new Vector3(position.x + platformDistance / 2, position.y, castle2.transform.position.z);
			lastSpawnedCastle = castle4;
			castleSpawned = true;
		}

        if (platformCount >= 30 && castle5 == null)
		{
			castle5 = GameObject.Find("castle5");
            castle5.transform.position = new Vector3(position.x + platformDistance / 2, position.y, castle2.transform.position.z);
            platformPrefab = platformSmallPrefab;
            platformDistance = 25.5f;
            lastSpawnedCastle = castle5;
			castleSpawned = true;
		}

        if (platformCount >= 40 && castle6 == null)
		{
			castle6 = GameObject.Find("castle6");
            castle6.transform.position = new Vector3(position.x + platformDistance / 2, position.y, castle2.transform.position.z);
			lastSpawnedCastle = castle6;
			castleSpawned = true;
		}

		if (!castleSpawned) {
			if (lastSpawnedCastle != null)
			{
				position = new Vector2(lastSpawnedCastle.transform.position.x + platformDistance, lastSpawnedCastle.transform.position.y);
				lastSpawnedCastle = null;
			}
			lastSpawnedPlatform = (GameObject)Instantiate (platformPrefab, position, Quaternion.identity);
            platformCount++;

			//Debug.Log ("Spawning platforms at : " + position);
			//Debug.Log ("xCameraBoundary is at: " + xCameraBoundary);
			if (position.x > xCameraBoundary) {
				//Debug.Log ("Adding platform to the queue: " + lastSpawnedPlatform.transform);
				platformQueue.Enqueue (lastSpawnedPlatform);
			}
		}
    }
}
