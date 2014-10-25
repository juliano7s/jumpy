﻿using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {

    public GameObject platformPrefab;
    public Transform player;
    public float newSpawnXThreshold = 100f; //threshold to spawn a new platform in relation to X boundary of the camera
    public float yPositionRangeBottom = 0f, yPositionRangeTop = 30f;

    private GameObject lastSpawnedPlatform;

    // Use this for initialization
    void Start () {
        //Debug.Log("Initializing platformSpawner");
        for (int i = 1; i < 6; i++)
        {
            //Debug.Log("Instantiating platform from prefab: " + platformPrefab);
            //Debug.Log("--- collider scale: " + ((BoxCollider2D) platformPrefab.collider2D).size);
            Vector2 platformSize = ((BoxCollider2D) platformPrefab.collider2D).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(player.position.x + i * 1.75f * platformSize.x, yRandom);
            SpawnPlatform(position);
        }
    }

    void Update() {
        //Debug.Log("Camera position: " + Camera.main.transform.position);
        //Debug.Log("Camera width: " + Camera.main.orthographicSize * Screen.width/Screen.height);
        float xCameraBoundary = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width/Screen.height;
        float currentXThreshold = Mathf.Abs(lastSpawnedPlatform.transform.position.x - xCameraBoundary);
        //Debug.Log ("xCameraBoundary: " + xCameraBoundary);
        //Debug.Log ("lastSpawnedPlatform.transform.position.x - xCameraBoundary =  " + lastSpawnedPlatform.transform.position.x + " - " + xCameraBoundary + " = " + currentXThreshold);
        if (currentXThreshold < newSpawnXThreshold)
        {
            Vector2 platformSize = ((BoxCollider2D) platformPrefab.collider2D).size;
            float yRandom = Random.Range(yPositionRangeBottom, yPositionRangeTop);
            Vector2 position = new Vector2(lastSpawnedPlatform.transform.position.x + 1.75f * platformSize.x, yRandom);
            Debug.Log("Spawning platform at: " + position);
            SpawnPlatform(position);
        }
    }

    void SpawnPlatform(Vector2 position) {
        lastSpawnedPlatform = (GameObject) Instantiate(platformPrefab, position, Quaternion.identity);
    }
}
