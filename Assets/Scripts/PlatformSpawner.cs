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
	private float platformDistance = 40.0f;
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
        //
        //scoreScriptObject = GameObject.Find("/score").GetComponent<Score>();

        platformPrefab = platformLargePrefab;

        //
        xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        platformQueue = new Queue();

		float lastSpawnedPosition = player.position.x;
        for (int i = 1; i < 4; i++)
        {
            //
            //
			//Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = i == 1 ? player.position.y - 12 : Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPosition + platformDistance, yRandom);
			lastSpawnedPosition += platformDistance;
            SpawnPlatform(position);
        }
        GameObject nextPlatform = platformQueue.Count > 0 ? (GameObject) platformQueue.Peek() : null;
        nextPlatformArrow.transform.position = new Vector3(xCameraBoundary, nextPlatform.transform.position.y, nextPlatformArrow.transform.position.z);
    }

    void Update() {
        //
        //
        xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        float currentXThreshold = Mathf.Abs(lastSpawnedPlatform.transform.position.x - xCameraBoundary);
        //
        //
        if (currentXThreshold < newSpawnXThreshold)
        {
            //Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPlatform.transform.position.x + platformDistance, yRandom);
            //
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
        
		//
		bool castleSpawned = false;

		if (platformCount >= 5 && castle2 == null)
		{			
			castle2 = GameObject.Find("castle2");
			castle2.transform.position = new Vector3(position.x + 17, position.y, castle2.transform.position.z);
            platformDistance = 45.0f;
			lastSpawnedCastle = castle2;
			castleSpawned = true;
		}

        if (platformCount >= 15 && castle3 == null)
		{			
			castle3 = GameObject.Find("castle3");
            castle3.transform.position = new Vector3(position.x + 17, position.y, castle2.transform.position.z);
            platformPrefab = platformMediumPrefab;
            platformDistance = 30.0f;
			lastSpawnedCastle = castle3;
			castleSpawned = true;
		}

        if (platformCount >= 25 && castle4 == null)
		{
			castle4 = GameObject.Find("castle4");
            castle4.transform.position = new Vector3(position.x + 15, position.y, castle2.transform.position.z);
            platformDistance = 35.0f;
			lastSpawnedCastle = castle4;
			castleSpawned = true;
		}

        if (platformCount >= 35 && castle5 == null)
		{
			castle5 = GameObject.Find("castle5");
            castle5.transform.position = new Vector3(position.x + 15, position.y, castle2.transform.position.z);
            platformPrefab = platformSmallPrefab;
            platformDistance = 25.0f;
            lastSpawnedCastle = castle5;
			castleSpawned = true;
		}

        if (platformCount >= 50 && castle6 == null)
		{
			castle6 = GameObject.Find("castle6");
            castle6.transform.position = new Vector3(position.x + 15, position.y, castle2.transform.position.z);
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

			//
			//
			if (position.x > xCameraBoundary) {
				//
				platformQueue.Enqueue (lastSpawnedPlatform);
			}
		}
    }
}
