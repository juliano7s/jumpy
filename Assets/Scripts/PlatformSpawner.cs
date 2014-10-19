using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public GameObject platformPrefab;
    public Transform player;

    // Use this for initialization
    void Start () {
        Debug.Log("Initializing platformSpawner");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Instantiating platform from prefab: " + platformPrefab);
            Debug.Log("--- collider scale: " + ((BoxCollider2D) platformPrefab.collider2D).size);
            Vector2 platformSize = ((BoxCollider2D) platformPrefab.collider2D).size;
            Vector2 position = new Vector2(player.position.x + i * 2 * platformSize.x, player.position.y - 2 * platformSize.y);
            Instantiate(platformPrefab, position, Quaternion.identity);
        }
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
