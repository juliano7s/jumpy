using UnityEngine;
using System.Collections.Generic;

public class JumpyController : MonoBehaviour
{
    public const float JUMP_MULTIPLIER = 6000f; //ammount to multiply jump vector
    public const float MAX_JUMP_FORCE = 4000f;
    public const float MAX_JUMP_ARROW_SCALE = 5f;
    public const float MOVING_THRESHOLD = 0.5f;     //below this value, object is considered stopped    
    public const float GROUND_RADIUS = 2.5f;       //radius of the ground check circle
    
    private bool moving = false;    //indicates whether object is moving or not
    private bool grounded = false; //indicates whether object is grounded or not
    public Transform groundCheck;  //position of the ground check circle    
    public LayerMask whatIsGround; //layers that are considered ground

    public bool isComboJump = false;

    private float jumpMultiplierX, jumpMultiplierY;
    private Vector2 jumpCmdStart;
    private Vector2 jumpCmdEnd;
    private Vector2 jumpVector;
    public GameObject jumpArrow;
    public GameObject jumpArrowBody;
    public GameObject jumpArrowHead;

    private Queue<Vector2> jumpCommands;       //jump commands queue
    private Queue<bool> jumpCombos;
    private bool mouseIsHoldedDown;

    public Animator anim;
    public Animator SpeechAnimator;
    
    public GUIText jumpDebugGuiText;

    public Score ScoreScriptObject;
    public bool ShowingTutorial = false;

    // Use this for initialization
    void Start ()
    {
        
        ScoreScriptObject = GameObject.Find ("score").GetComponent<Score> ();
        mouseIsHoldedDown = false;
        jumpCommands = new Queue<Vector2>();
        jumpCombos = new Queue<bool>();
        anim = GetComponent<Animator>();
        jumpArrowBody.GetComponent<Renderer>().enabled = false;
        jumpArrowHead.GetComponent<Renderer>().enabled = false;
#if DEBUG
        jumpDebugGuiText = GameObject.Find("debug/jumpVector").GetComponent<GUIText>();
#endif
        jumpMultiplierY = JUMP_MULTIPLIER;
        jumpMultiplierX = JUMP_MULTIPLIER;

        bool isFirstTime = PlayerPrefs.GetInt("FirstTime") > 0 ? false : true;
        if (isFirstTime) {
            SpeechAnimator.SetBool("showTutorial", true);
            PlayerPrefs.SetInt("FirstTime", 1);
        } else {
            int speak = Random.Range(1,6);
            if (speak == 1) {
                SpeechAnimator.SetBool("speak", true);
            }
        }
    }

    void OnLevelWasLoaded(int level)
    {
        
        if (level == 1) {
            int speak = Random.Range(1,6);
            
            if (speak == 1) {
                SpeechAnimator.SetBool("speak", true);
            }
        }
    }
        
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, GROUND_RADIUS, whatIsGround);
        moving = GetComponent<Rigidbody2D>().velocity.magnitude > MOVING_THRESHOLD ? true : false;

        //dequeue the last jump command and add the force
        if (jumpCommands.Count > 0) {
            if (!moving && grounded) {
                Vector2 nextJump = (Vector2) jumpCommands.Dequeue();
                isComboJump = (bool) jumpCombos.Dequeue();
                GetComponent<AudioSource>().Play();
            
                //
                //

                GetComponent<Rigidbody2D>().AddForce(nextJump);
            }
        } else if (ScoreScriptObject.comboCount > 0 && !moving) {
            ScoreScriptObject.comboCount = 0;
        }
    }

    void Update ()
    {
        if (ShowingTutorial)
            return;

#if UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
#else
        if (Input.GetMouseButtonDown(0))
#endif
        {
#if UNITY_ANDROID
            jumpCmdStart = Input.GetTouch(0).position;
#else
            jumpCmdStart = Input.mousePosition;
#endif
            jumpArrow.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(jumpCmdStart).x,
                                                       Camera.main.ScreenToWorldPoint(jumpCmdStart).y,
                                                       jumpArrow.transform.position.z);
            mouseIsHoldedDown = true;
            jumpArrowBody.GetComponent<Renderer>().enabled = true;
            jumpArrowHead.GetComponent<Renderer>().enabled = true;
        }
#if UNITY_ANDROID
        if (mouseIsHoldedDown && Input.touchCount == 1)
#else
        if (mouseIsHoldedDown)
#endif
        {
#if UNITY_ANDROID
            jumpCmdEnd = Input.GetTouch(0).position;
#else
            jumpCmdEnd = Input.mousePosition;
#endif
#if DEBUG
            if (jumpDebugGuiText != null)
                jumpDebugGuiText.text = jumpCmdStart.ToString() + " -> " + jumpCmdEnd.ToString();
#endif
            jumpVector = jumpCmdStart - jumpCmdEnd;
            Vector3 lookPos = Camera.main.ScreenToWorldPoint(jumpCmdStart);
            lookPos = lookPos - Camera.main.ScreenToWorldPoint(jumpCmdEnd);
            float angle = Mathf.Atan2(lookPos.x, lookPos.y) * Mathf.Rad2Deg;
            angle -= 90;
            jumpArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            
            jumpVector.x = jumpVector.x / Screen.width;
            jumpVector.y = jumpVector.y / Screen.height;
            jumpVector.x = jumpVector.x * jumpMultiplierX;
            jumpVector.y = jumpVector.y * jumpMultiplierY;
            //
            //
            
            if (jumpVector.x > MAX_JUMP_FORCE)
                jumpVector.x = Mathf.Sign (jumpVector.x) * MAX_JUMP_FORCE;
            if (jumpVector.y > MAX_JUMP_FORCE)
                jumpVector.y = Mathf.Sign (jumpVector.y) * MAX_JUMP_FORCE;
            
            //
            float scaleAmount = jumpVector.magnitude / MAX_JUMP_FORCE * MAX_JUMP_ARROW_SCALE;
            jumpArrowBody.transform.localScale = new Vector3(scaleAmount, jumpArrow.transform.localScale.y, jumpArrow.transform.localScale.z);
        }
#if UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
#else
        if (Input.GetMouseButtonUp(0))
#endif
        
        {
            AddJump(jumpVector);
#if DEBUG
            if (jumpDebugGuiText != null)
                jumpDebugGuiText.text += " : (" + jumpVector.x + ", " + jumpVector.y + ") " + jumpVector.magnitude;
#endif
            mouseIsHoldedDown = false;
            jumpArrowBody.GetComponent<Renderer>().enabled = false;
            jumpArrowHead.GetComponent<Renderer>().enabled = false;
        }
        
        anim.SetBool("grounded", (grounded || !moving));
        anim.SetBool("isPreparingJump", mouseIsHoldedDown);
    }

	void OnCollisionEnter2D (Collision2D collision)
	{
		ParticleSystem dustPS = transform.GetChild(2).GetComponent<ParticleSystem>();
		dustPS.Play();

        if (collision.gameObject.transform.tag.Equals("Castle") && collision.collider.GetType().ToString().Equals("UnityEngine.BoxCollider2D")) {
            if (collision.gameObject.transform.name.Equals("castle6")) {
                SpeechAnimator.SetBool("castleFinal", true);
                SpeechAnimator.SetBool("speak", true);
            } else {
                SpeechAnimator.SetBool("castle", true);
                SpeechAnimator.SetBool("speak", true);
            }
        }
	}

    void AddJump(Vector2 jump)
    {
        Vector3 vector = new Vector3(jump.x, jump.y, transform.position.z);
        if (!grounded || moving || jumpCommands.Count > 0)
            jumpCombos.Enqueue(true);
        else
            jumpCombos.Enqueue(false);

        jumpCommands.Enqueue(vector);
    }
}
