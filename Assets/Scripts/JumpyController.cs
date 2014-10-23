﻿using UnityEngine;
using System.Collections.Generic;

public class JumpyController : MonoBehaviour
{
    public const float JUMP_MULTIPLIER = 10f; //ammount to multiply jump vector
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

    public Animator anim;

    // Use this for initialization
    void Start ()
    {
        mouseIsHoldedDown = false;
        jumpCommands = new Queue<Vector2>();
        jumpCombos = new Queue<bool>();
        anim = GetComponent<Animator>();
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

#if UNITY_ANDROID
    void Update ()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            jumpCmdStart = Input.mousePosition;
            mouseIsHoldedDown = true;
        }
        
        if (mouseIsHoldedDown)
        {
            jumpCmdEnd = Input.mousePosition;
            jumpVector = jumpCmdStart - jumpCmdEnd;
        }
        
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (moving || (!moving && jumpCommands.Count > 0))
                jumpCombos.Enqueue(true);
            else
                jumpCombos.Enqueue(false);
            
            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            mouseIsHoldedDown = false;
        }
        anim.SetBool("grounded", (grounded || !moving));
        anim.SetBool("isPreparingJump", mouseIsHoldedDown);
    }
#endif

#if UNITY_EDITOR || UNITY_WEBPLAYER
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
        anim.SetBool("grounded", (grounded || !moving));
        anim.SetBool("isPreparingJump", mouseIsHoldedDown);
    }
#endif
}
