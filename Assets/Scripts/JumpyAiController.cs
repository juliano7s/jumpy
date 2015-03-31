using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class JumpyAiController : MonoBehaviour
{

    public const float MAX_FORCE = 5000f;
    public const float GROUND_RADIUS = 1.52f;       //radius of the ground check circle
    public const float MOVING_THRESHOLD = 0.5f;     //below this value, object is considered stopped    
    public const float JUMP_TIMER_TIME = 2f;
    public const float PREPARE_JUMP_TIMER_TIME = 1.5f;

    private bool grounded = false; //indicates whether object is grounded or not
    private bool moving = false; //indicates whether object is moving or not
    public Transform groundCheck;  //position of the ground check circle    
    public LayerMask whatIsGround; //layers that are considered ground

    private float jumpTimer = JUMP_TIMER_TIME;
    private float prepareJumpTimer = PREPARE_JUMP_TIMER_TIME;
    public Animator anim;

    public GameObject googlePlayDebug;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();

        //googlePlayDebug.GetComponent<TextMesh>().text = "authenticating on google play";
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            
            //googlePlayDebug.GetComponent<TextMesh>().text = "User authenticate google play games: " + success;
        });
    }
    
    void Update() {
        // get the main camera's GUILayer:
        GUILayer gLayer = Camera.main.GetComponent< GUILayer>();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                // Get the GUIElement touched, if any:
                GUIElement button = gLayer.HitTest(Input.GetTouch(i).position);
                if (button) // if any GUIElement touched...
                {
                    // call OnMouseDown in its scripts:
                    button.SendMessage("OnMouseDown");
                }
            }
        }
    }

    void OnLevelWasLoaded(int level)
    {

    }
    
    // Update is called once per frame
    void FixedUpdate ()
    {

        grounded = Physics2D.OverlapCircle (groundCheck.position, GROUND_RADIUS, whatIsGround);
        moving = GetComponent<Rigidbody2D>().velocity.magnitude > MOVING_THRESHOLD ? true : false;
        jumpTimer -= Time.deltaTime;
        anim.SetBool("grounded", grounded);

        if (!moving && jumpTimer <= 0)
        {
            prepareJumpTimer -= Time.deltaTime;
            if (prepareJumpTimer <= 0)
            {
                anim.SetBool("isPreparingJump", false);
                float xForce = Random.Range (-MAX_FORCE, MAX_FORCE);
                float yForce = Random.Range (0, MAX_FORCE);
                Vector2 randomForce = new Vector2 (xForce, yForce);
                GetComponent<Rigidbody2D>().AddForce (randomForce);
                GetComponent<AudioSource>().Play();
                prepareJumpTimer = PREPARE_JUMP_TIMER_TIME;
                jumpTimer = JUMP_TIMER_TIME;
            } else 
            {
                anim.SetBool("isPreparingJump", true);
            }
        }
    }
}
