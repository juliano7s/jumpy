using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public float xMargin = 5f;        // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 5f;        // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 2f;        // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 2f;        // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.
    public float xOffset = 15f;     // How much to the left the player will appear on the camera

    private Transform player;        // Reference to the player's transform.

    private GUIText cameraDebugGuiText;
    private string screenProperties;
    private string cameraPosition;

    void Awake ()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;		
#if DEBUG
        //cameraDebugGuiText = GameObject.Find("debug/camera").guiText;
        //screenProperties = "Height: " + 2 * camera.orthographicSize + " Width: " + 2 * camera.orthographicSize * camera.aspect + "\n";
        //screenProperties += "Bottom: " + (transform.position.y - camera.orthographicSize) +
        //    " Top: " + (transform.position.y + camera.orthographicSize) + "\n";
        //screenProperties += "Left: " + (transform.position.y - camera.orthographicSize * camera.aspect) +
        //    " Right: " + (transform.position.y + camera.orthographicSize * camera.aspect) + "\n";
#endif
    }


    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void FixedUpdate ()
    {
        TrackPlayer();
        GameObject sky = GameObject.Find("sky");
        sky.transform.position = new Vector3(transform.position.x, transform.position.y, sky.transform.position.z);		
    }
    
    
    void TrackPlayer ()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        //if(CheckXMargin())    // CHECKING MARGINS HAVE A WEIRD EFFECT WHEN PLAYER PASSES THE CAMERA POSITION
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        targetX = Mathf.Lerp(transform.position.x, player.position.x + xOffset, xSmooth * Time.deltaTime);

        // If the player has moved beyond the y margin...
        //if(CheckYMargin())    // CHECKING MARGINS HAVE A WEIRD EFFECT WHEN PLAYER PASSES THE CAMERA POSITION
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
        targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
#if DEBUG
        cameraPosition = "Camera position: (" + targetX + ", " + targetY + ")";
        cameraDebugGuiText.text = screenProperties + cameraPosition;
#endif
    }
}
