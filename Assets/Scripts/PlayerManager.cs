using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputSystem playerInputSys; //input system reference

    public Rigidbody2D rb;
    public Animator anim;
    public float moveSpeed = 1f;
    [Header("Jumping")]
    public float jumpforce = 1f;
    public float lowjumpMultiplier = 2f;
    public float fallMultiplier = 3f;
    public bool isGrounded = true;
    public bool isFalling = false;

    private float coyoteTime = 0.1f;
    [SerializeField] private float coyoteTimeCounter = 0.1f;
    public float rayDist = 1f;
    public float rayOffset = 0.1f;



    void Awake()
    {
        playerInputSys = new PlayerInputSystem(); //initialising the input system

    }
    void OnEnable()
    {
        playerInputSys.Enable(); //needed for the input system
    }
    void OnDisable()
    {
        playerInputSys.Disable(); //needed for the input system
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerInput = playerInputSys.Player.Move.ReadValue<Vector2>(); //reads the player's input from the input system and turns it into a vector2

        rb.linearVelocity = new Vector2(playerInput.x * moveSpeed, rb.linearVelocity.y);

        if (playerInput.x != 0)
        {
            //anim.SetBool("isWalking", true);
            if (playerInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f); //rotates the player left or right based on input
            }
            else if (playerInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            /*
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }*/

        }
        else
        {
            //anim.SetBool("isWalking", false);
            //walkingSound.Stop();
        }

        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y - rayOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(rayPos, Vector2.down, rayDist, LayerMask.GetMask("Ground")) || Physics2D.Raycast(rayPos, Vector2.down, rayDist, LayerMask.GetMask("MovableObjects"));
        Debug.DrawRay(rayPos, Vector2.down * rayDist, Color.red);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;

        }

        if (playerInputSys.Player.Jump.WasPressedThisFrame() && coyoteTimeCounter > 0f)
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0;
        }

        if (rb.linearVelocity.y < 0f) //falling
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            isFalling = true;
        }
        else if (rb.linearVelocity.y > 0f && !playerInputSys.Player.Jump.IsPressed()) //low jump
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowjumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        if (rb.linearVelocity.y >= 0f)
        {
            isFalling = false;
        }

        /*
    UpdateAnims();


    if (FreezePlayer)
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
    else
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}

void UpdateAnims()
{
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isFalling", isFalling);
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
    }
}*/

    }
}
