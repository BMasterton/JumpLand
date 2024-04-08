using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPt;
    [SerializeField] Transform respawnPt;
    [SerializeField] GameManager gameController;

    private float horizInput;           // store horizontal input for used in FixedUpdate()
    private float moveSpeed = 450.0f;   // 4.5 * 100 newtons

    private float jumpHeight = 3.0f;    // height of jump in units
    private float jumpTime = 0.75f;     // time of jump in seconds
    private float initialJumpVelocity;  // calculated jump velocity

    private bool isGrounded = false;    // true if player is grounded
    [SerializeField] private Transform groundCheckPoint;    // draw a circle around this to check ground
    [SerializeField] private LayerMask groundLayerMask;     // a layer for all things ground
    private float groundCheckRadius = 0.3f;                 // radius of ground check circle

    private int jumpMax = 2;            // # of jumps player can do without touching ground
    private int jumpsAvailable = 0;     // current jumps available to player

    private bool facingRight = true;    // true if facing right

    private int maxHealth = 5;
    private int currentHealth;

    private void Start()
    {
        setBulletBackToNormal();
        currentHealth = maxHealth;
        // calculate gravity using gravity formula
        float timeToApex = jumpTime / 2.0f;
        float gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        Debug.Log("gravity:" + gravity);

        // adjust gravity scale of player based on gravity required for jumpHeight & jumpTime
        rbody.gravityScale = gravity / Physics2D.gravity.y;

        // calculate jump velocity req'd for jumpHeight & jumpTime
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }


    private void setBulletBackToNormal()
    {
        SpriteRenderer sprite = bullet.GetComponent<SpriteRenderer>();
        sprite.color = Color.yellow;
        Transform alsoSprite = bullet.GetComponent<Transform>();
        alsoSprite.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_HEALTH, this.OnPickupHealth);
        Messenger<string>.AddListener(GameEvent.POWER_UP, this.powerupChoice);
    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_HEALTH, this.OnPickupHealth);
        Messenger<string>.RemoveListener(GameEvent.POWER_UP, this.powerupChoice);
    }

    IEnumerator powerUpTimer(string powerup)
    {
        if (powerup == "Apple") { 
            SpriteRenderer sprite = bullet.GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            yield return new WaitForSeconds(15);
            sprite.color = Color.yellow; 
        }
        else if (powerup == "Melon")
        {
            Transform sprite = bullet.GetComponent<Transform>();
            sprite.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(15);
            sprite.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (powerup == "Cherry")
        {
            rbody.gravityScale /= 2;
              yield return new WaitForSeconds(15);
            rbody.gravityScale *= 2;
        }
        else if (powerup == "Pineapple")
        {
            moveSpeed *= 2;
             yield return new WaitForSeconds(15);
            moveSpeed /= 2; 
        }


    }

    private void powerupChoice(string powerup)
    {
        StartCoroutine(powerUpTimer(powerup));
    }

    void Update()
    {
        // read & store horizontal input
        horizInput = Input.GetAxis("Horizontal");

        // determine if player is running (for animator param)
        bool isRunning = horizInput > 0.01 || horizInput < -0.01;
        anim.SetBool("isRunning", isRunning);   // notify animator

        // determine if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) && rbody.velocity.y < 0.01;
        
        anim.SetBool("isGrounded", isGrounded); // notify animator

        // reset jumps if grounded
        if (isGrounded)
        {
            jumpsAvailable = jumpMax;
        }


        if (Input.GetButtonDown("Fire1") )
        {
            Fire();
        }

        // if jump is triggered & available - go for it
        if (Input.GetButtonDown("Jump") && jumpsAvailable > 0)
        {
            Jump();
        }

        // Flip player if appropriate
        if ((facingRight && horizInput < -0.01) ||
            (!facingRight && horizInput > 0.01))
        {
            Flip();
        }
    }

    public void OnPickupHealth(int healthAdded)
    {
        Debug.Log("Entered the onPickpHealth");
        currentHealth += healthAdded;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        float healthPercent = ((float)currentHealth) / maxHealth;
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, healthPercent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "respawn" )
        { 
        respawnPt = collision.transform;
        }
            
        if (collision.gameObject.tag == "Bounds" && currentHealth != 0)
        {
            currentHealth--;
            transform.position = respawnPt.position;
        }
        else if(collision.gameObject.tag == "Bounds" && currentHealth == 0)
        {
            // end game screen with restart and try again.
            Application.Quit();
        }
       

    }

    private void OnDrawGizmos()
    {
        // draw the ground check sphere in the Scene
        Gizmos.DrawSphere(groundCheckPoint.position, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        // move the player (use horizontal input, but maintain existing y velocity)
        float xVel = horizInput * moveSpeed * Time.deltaTime;
        rbody.velocity = new Vector2(xVel, rbody.velocity.y);
    }

    void Jump()
    {
        // tell the player to jump
        rbody.velocity = new Vector2(rbody.velocity.x, initialJumpVelocity);
        jumpsAvailable--;
        if(jumpsAvailable == 1) { 
            anim.SetTrigger("jump");    // notify animator
        }else if (jumpsAvailable == 0)
        {
            anim.SetTrigger("doubleJump");
        }
       
    }

    void Flip()
    {
        // flip the direction the player is facing
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180);
    }

    IEnumerator destoryBall(GameObject go)
    {
        yield return new WaitForSeconds(1);
        Destroy(go);
    }

    void Fire()
    {
            GameObject go = Instantiate(bullet, bulletSpawnPt.position, bullet.transform.rotation);
        if (facingRight)
        {
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200 * 5);
        }
        else
        {
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200 * 5);
        }
           
        destoryBall(go);
    }

    public void Hit()
    {
        currentHealth -= 1;
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, (currentHealth / maxHealth));
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            //Debug.Break();
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }




        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "OneWayPlatform")
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true); // not triggering grounded for some reason even though the anim in being set 
            jumpsAvailable = jumpMax;

        }
        else if (collision.gameObject.tag == "FatBird" || 
            collision.gameObject.tag == "AngryPig" ||
            collision.gameObject.tag == "Ghost"
            && currentHealth != 0)
        {
            Hit();
            anim.SetTrigger("ouch");
        }
    }
}
