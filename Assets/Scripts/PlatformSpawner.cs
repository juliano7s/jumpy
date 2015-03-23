using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {

    public GameObject platformLargePrefab;
    public GameObject platformMediumPrefab;
    public GameObject platformSmallPrefab;
    private GameObject platformPrefab;

    private Score scoreScriptObject;

    public Transform player;
    public float newSpawnXThreshold = 100f; //threshold to spawn a new platform in relation to X boundary of the camera
    public float yPositionRangeBottom = 0f, yPositionRangeTop = 30f;
	private float platformDistance = 45.5f;
    private float xCameraBoundary;

    private GameObject lastSpawnedPlatform;
    private Queue platformQueue;
    public GameObject nextPlatformArrow;

    // Use this for initialization
    void Start () {

        scoreScriptObject = GameObject.Find("score").GetComponent<Score>();

        platformPrefab = platformLargePrefab;

        //Debug.Log("Initializing platformSpawner");
        xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        platformQueue = new Queue();
        for (int i = 1; i < 4; i++)
        {
            //Debug.Log("Instantiating platform from prefab: " + platformPrefab);
            //Debug.Log("--- collider scale: " + ((BoxCollider2D) platformPrefab.collider2D).size);
            Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(player.position.x + i * platformDistance, yRandom);
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
            Vector2 platformSize = ((BoxCollider2D) platformPrefab.GetComponent<Collider2D>()).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPlatform.transform.position.x + platformDistance, yRandom);
            Debug.Log("Spawning platform at: " + position);
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
        lastSpawnedPlatform = (GameObject) Instantiate(platformPrefab, position, Quaternion.identity);

        if (scoreScriptObject.score >= 7 && platformPrefab != platformMediumPrefab)
        {
            platformPrefab = platformMediumPrefab;
        }
        
        if (scoreScriptObject.score >= 17 && platformPrefab != platformSmallPrefab)
        {
            platformPrefab = platformSmallPrefab;
        }

        Debug.Log("Spawning platforms at : " + position);
        Debug.Log("xCameraBoundary is at: " + xCameraBoundary);
        if (position.x > xCameraBoundary)
        {
            Debug.Log("Adding platform to the queue: " + lastSpawnedPlatform.transform);
            platformQueue.Enqueue(lastSpawnedPlatform);
        }
    }
}
