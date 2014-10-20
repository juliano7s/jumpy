using UnityEngine;
using System.Collections.Generic;

public class JumpyController : MonoBehaviour
{
    public const float JUMP_MULTIPLIER = 100; //ammount to multiply jump vector
    public const float MOVING_THRESHOLD = 0.5f; //below that object is considered stopped    
    public const float GROUND_RADIUS = 0.2f;       //radius of the ground check circle
    
    private bool moving = false;    //indicates whether object is moving or not
    private bool grounded = false; //indicates whether object is grounded or not
    public Transform groundCheck;  //position of the ground check circle    
    public LayerMask whatIsGround; //layers that are considered ground

    public bool isComboJump = false;

    private Vector2 jumpCmdStart;
    private Vector2 jumpCmdEnd;
    private Vector2 jumpVector;
    
    private Queue<Vector2> jumpCommands;       //jump commands queue
    private Queue<bool> jumpCombos;
    private bool mouseIsHoldedDown;

    // Use this for initialization
    void Start ()
    {
         mouseIsHoldedDown = false;
         jumpCommands = new Queue<Vector2>();
         jumpCombos = new Queue<bool>();
    }
    
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, GROUND_RADIUS, whatIsGround);
        moving = rigidbody2D.velocity.magnitude > MOVING_THRESHOLD ? true : false;

        //dequeue the last jump command and add the force
        if (jumpCommands.Count > 0 && !moving)
        {
            Vector2 nextJump = (Vector2) jumpCommands.Dequeue();            
            isComboJump = (bool) jumpCombos.Dequeue();
            Debug.Log ("isComboJump = " + isComboJump);
            audio.Play();
            rigidbody2D.AddForce(nextJump * JUMP_MULTIPLIER);
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
            if (moving || (!moving && jumpCommands.Count > 0))
                jumpCombos.Enqueue(true);
            else
                jumpCombos.Enqueue(false);
                
            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            //Debug.Log("jumpVector magnitude: " + jumpVector.magnitude);
            mouseIsHoldedDown = false;
        }
    }
}
