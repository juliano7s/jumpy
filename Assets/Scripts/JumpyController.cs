using UnityEngine;
using System.Collections.Generic;

public class JumpyController : MonoBehaviour
{
    public const float JUMP_MULTIPLIER = 20; //ammount to multiply jump vector
    public const float MAX_JUMP_FORCE = 4000f;
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
        if (jumpCommands.Count > 0 && (!moving || grounded))
        {
            Vector2 nextJump = (Vector2) jumpCommands.Dequeue();
            isComboJump = (bool) jumpCombos.Dequeue();
            audio.Play();
            nextJump = nextJump * JUMP_MULTIPLIER;
            Debug.Log ("jumpForce: " + nextJump);
            Debug.Log ("jumpForce magnitude: " + nextJump.magnitude);
            
            if (nextJump.x > MAX_JUMP_FORCE)
                nextJump.x = Mathf.Sign (nextJump.x) * MAX_JUMP_FORCE;
            if (nextJump.y > MAX_JUMP_FORCE)
                nextJump.y = Mathf.Sign (nextJump.y) * MAX_JUMP_FORCE;

            Debug.Log ("jumpForce: " + nextJump);
            Debug.Log ("jumpForce magnitude: " + nextJump.magnitude);

            rigidbody2D.AddForce(nextJump);
        }
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            jumpCmdStart = Input.mousePosition;
            mouseIsHoldedDown = true;
        }

        if (mouseIsHoldedDown)
        {
            jumpCmdEnd = Input.mousePosition;
            jumpVector = jumpCmdStart - jumpCmdEnd;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (moving || (!moving && jumpCommands.Count > 0))
                jumpCombos.Enqueue(true);
            else
                jumpCombos.Enqueue(false);
                
            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            mouseIsHoldedDown = false;
        }
    }
}
