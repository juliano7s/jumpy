using UnityEngine;
using System.Collections.Generic;

public class JumpyController : MonoBehaviour
{
    public const float JUMP_MULTIPLIER = 6000f; //ammount to multiply jump vector
    public const float MAX_JUMP_FORCE = 4000f;
    public const float MOVING_THRESHOLD = 0.5f;     //below this value, object is considered stopped    
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
    
    public GUIText jumpDebugGuiText;

    // Use this for initialization
    void Start ()
    {
        mouseIsHoldedDown = false;
        jumpCommands = new Queue<Vector2>();
        jumpCombos = new Queue<bool>();
        anim = GetComponent<Animator>();
        jumpDebugGuiText = GameObject.Find("debug/jumpVector").guiText;
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
            jumpDebugGuiText.text = jumpCmdStart.ToString() + " -> " + jumpCmdEnd.ToString();
            jumpVector = jumpCmdStart - jumpCmdEnd;
        }
        
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (moving || (!moving && jumpCommands.Count > 0))
                jumpCombos.Enqueue(true);
            else
                jumpCombos.Enqueue(false);

            jumpVector.x = jumpVector.x / Screen.width;
            jumpVector.y = jumpVector.y / Screen.height;
            jumpVector = jumpVector * JUMP_MULTIPLIER;
            Debug.Log ("jumpForce: " + jumpVector);
            Debug.Log ("jumpForce magnitude: " + jumpVector.magnitude);
            
            if (jumpVector.x > MAX_JUMP_FORCE)
                jumpVector.x = Mathf.Sign (jumpVector.x) * MAX_JUMP_FORCE;
            if (jumpVector.y > MAX_JUMP_FORCE)
                jumpVector.y = Mathf.Sign (jumpVector.y) * MAX_JUMP_FORCE;

            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            jumpDebugGuiText.text += " : (" + jumpVector.x + ", " + jumpVector.y + ") " + jumpVector.magnitude;
            mouseIsHoldedDown = false;
        }
        anim.SetBool("grounded", (grounded || !moving));
        anim.SetBool("isPreparingJump", mouseIsHoldedDown);
    }
#endif

#if UNITY_EDITOR
#if UNITY_WEBPLAYER
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
            jumpDebugGuiText.text = jumpCmdStart.ToString() + " -> " + jumpCmdEnd.ToString();
            jumpVector = jumpCmdStart - jumpCmdEnd;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (moving || (!moving && jumpCommands.Count > 0))
                jumpCombos.Enqueue(true);
            else
                jumpCombos.Enqueue(false);

            jumpVector.x = jumpVector.x / Screen.width;
            jumpVector.y = jumpVector.y / Screen.height;
            jumpVector = jumpVector * JUMP_MULTIPLIER;
            Debug.Log ("jumpForce: " + jumpVector);
            Debug.Log ("jumpForce magnitude: " + jumpVector.magnitude);

            if (jumpVector.x > MAX_JUMP_FORCE)
                jumpVector.x = Mathf.Sign (jumpVector.x) * MAX_JUMP_FORCE;
            if (jumpVector.y > MAX_JUMP_FORCE)
                jumpVector.y = Mathf.Sign (jumpVector.y) * MAX_JUMP_FORCE;
            
            jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack
            jumpDebugGuiText.text += " : (" + jumpVector.x + ", " + jumpVector.y + ") " + jumpVector.magnitude;
            mouseIsHoldedDown = false;
        }
        
        anim.SetBool("grounded", (grounded || !moving));
        anim.SetBool("isPreparingJump", mouseIsHoldedDown);
    }
#endif
#endif
}
