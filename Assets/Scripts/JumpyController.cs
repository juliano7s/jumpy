using UnityEngine;
using System.Collections;

public class JumpyController : MonoBehaviour
{
    bool moving = false;
    bool grounded = false;
    public Transform groundCheck;  //position of the ground check circle
    float groundRadius = 0.2f;       //radius of the ground check circle
    public LayerMask whatIsGround; //layers that are considered ground
        
    private Vector2 jumpCmdStart;
    private Vector2 jumpCmdEnd;
    private Vector2 jumpVector;
    public float jumpMultiplier = 100;
    private Queue jumpCommands;       //jump commands queue
    private bool mouseIsHoldedDown;

    // Use this for initialization
    void Start ()
    {
         mouseIsHoldedDown = false;
         jumpCommands = new Queue();
    }
    
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        moving = rigidbody2D.velocity.magnitude > 0.0f ? true : false;

        //dequeue the last jump command and add the force
        if (jumpCommands.Count > 0 && !moving)
        {
            Vector2 nextJump = (Vector2) jumpCommands.Dequeue();
            Vector2 nextJumpForce = new Vector2(nextJump.x * jumpMultiplier, nextJump.y * jumpMultiplier);
            rigidbody2D.AddForce(nextJumpForce);
        }

    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseStart = Camera.main.ScreenPointToRay(Input.mousePosition);
            jumpCmdStart = new Vector2(mouseStart.origin.x, mouseStart.origin.y);
            mouseIsHoldedDown = true;
        }
        
        if (mouseIsHoldedDown)
        {
            Ray mouseEnd = Camera.main.ScreenPointToRay(Input.mousePosition);
            jumpCmdEnd = new Vector2(mouseEnd.origin.x, mouseEnd.origin.y);
            jumpVector = new Vector2(jumpCmdStart.x - jumpCmdEnd.x, jumpCmdStart.y - jumpCmdEnd.y);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            Debug.Log("jumpVector magnitude: " + jumpVector.magnitude);
            mouseIsHoldedDown = false;
        }
    }
}
