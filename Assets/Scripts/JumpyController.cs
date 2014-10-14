using UnityEngine;
using System.Collections;

public class JumpyController : MonoBehaviour
{
	private Vector2 jumpCmdStart;
	private Vector2 jumpCmdEnd;
	private Vector2 jumpVector;
	private bool mouseIsDown;

    // Use this for initialization
    void Start ()
    {
 		mouseIsDown = false;
    }

    void Update ()
    {    
    	if (Input.GetMouseButtonDown(0))
    	{
			Ray mouseStart = Camera.main.ScreenPointToRay(Input.mousePosition);   		
    		jumpCmdStart = new Vector2(mouseStart.origin.x, mouseStart.origin.y);
			Debug.Log("Left click down at: " + jumpCmdStart);
			mouseIsDown = true;
    	}
    	
    	if (mouseIsDown)
    	{
			Ray mouseEnd = Camera.main.ScreenPointToRay(Input.mousePosition);   		
			jumpCmdEnd = new Vector2(mouseEnd.origin.x, mouseEnd.origin.y);
			jumpVector = new Vector2(Mathf.Abs(jumpCmdStart.x - jumpCmdEnd.x),
				Mathf.Abs(jumpCmdStart.y - jumpCmdEnd.y));
			if (jumpVector.x > 1 || jumpVector.y > 1)
				Debug.Log("Jump cmd hold: deltaX = " + jumpVector.x + ", deltaY = " + jumpVector.y);			
    	}
    	
		if (Input.GetMouseButtonUp(0))
		{			
			Debug.Log("Left click up at: " + jumpCmdEnd);			
			Debug.Log("Jump cmd release: deltaX = " + jumpVector.x + ", deltaY = " + jumpVector.y);			
			mouseIsDown = false;
		} 
    }
}
