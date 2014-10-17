using UnityEngine;
using System.Collections;

public class JumpyController : MonoBehaviour
{
	bool moving = false;
	bool grounded = false;
	public Transform groundCheck;  //position of the ground check circle
	float groundRadius = 0.2f;	   //radius of the ground check circle
	public LayerMask whatIsGround; //layers that are considered ground
		
	private Vector2 jumpCmdStart;
	private Vector2 jumpCmdEnd;
	private Vector2 jumpVector;
	public float jumpMultiplier = 100;
	private Queue jumpCommands;	   //jump commands queue
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
    	if (rigidbody2D.velocity.x > 0 || rigidbody2D.velocity.y > 0)
    	{
    		moving = true;
			//Debug.Log("moving true = " + moving);
    	}
    	else
    	{
    		moving = false;
			//Debug.Log("moving false = " + moving);
    	}
    }

    void Update ()
    {
    	//pops the last jump command and add the force
        if (jumpCommands.Count > 0 && moving == false)
        {
        	Debug.Log("moving = " + moving);
        	Vector2 nextJump = (Vector2) jumpCommands.Dequeue();
			Vector2 nextJumpForce = new Vector2(nextJump.x * jumpMultiplier, nextJump.y * jumpMultiplier);
			Debug.Log("Jump force added: " + nextJumpForce);			
        	rigidbody2D.AddForce(nextJumpForce);
        }
        
    	if (Input.GetMouseButtonDown(0))
    	{
			Ray mouseStart = Camera.main.ScreenPointToRay(Input.mousePosition);   		
    		jumpCmdStart = new Vector2(mouseStart.origin.x, mouseStart.origin.y);
			//Debug.Log("Left click down at: " + jumpCmdStart);
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
			//Debug.Log("Left click up at: " + jumpCmdEnd);			
			//Debug.Log("Jump cmd release: deltaX = " + jumpVector.x + ", deltaY = " + jumpVector.y);
			Debug.Log("Jump cmd release: " + jumpVector);
			jumpCommands.Enqueue(jumpVector); //Add a jump command to the stack					
			mouseIsHoldedDown = false;
		}		
    }
}
