using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    /// Non Audio Serialized Fields   /// 
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPt;
    [SerializeField] Transform respawnPt;
    [SerializeField] GameManager gameController;
    [SerializeField] UIController ui;
    [SerializeField] Transform fakeHouseSpawn;

    /// Audio Clips ///
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip damageTakenSound;
    [SerializeField] private AudioClip fallOffMapSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip spikeTrapSound;
    [SerializeField] private AudioClip gameWinSound;
    [SerializeField] private AudioClip gameWinMusic;
    [SerializeField] private AudioSource audioSrc;
    private float horizInput;           // store horizontal input for used in FixedUpdate()
    private float moveSpeed = 450.0f;   // 4.5 * 100 newtons

    private float jumpHeight = 3.0f;    // height of jump in units
    private float jumpTime = 0.75f;     // time of jump in seconds
    private float initialJumpVelocity;  // calculated jump velocity

    // IsGrounded Variables
    private bool isGrounded = false;    // true if player is grounded
    [SerializeField] private Transform groundCheckPoint;    // draw a circle around this to check ground
    [SerializeField] private LayerMask groundLayerMask;     // a layer for all things ground
    private float groundCheckRadius = 0.3f;                 // radius of ground check circle

    //jumping variables
    private int jumpMax = 2;            // # of jumps player can do without touching ground
    private int jumpsAvailable = 0;     // current jumps available to player

    //shooting and flip variables 
    private bool facingRight = true;    // true if facing right

    //health variables
    private int maxHealth = 5;
    private int currentHealth;
    private int maxLives = 3;
    private int playerLives = 3;

    //keys
    private bool hasSpikeKey = false;
    private bool hasEnemyKey = false;
    private bool hasMazeKey = false;

    //invincible
    private bool isInvulnerable = false;


    private void Start()
    {
        setBulletBackToNormal();
        currentHealth = maxHealth;
        // calculate gravity using gravity formula
        float timeToApex = jumpTime / 2.0f;
        float gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
       // Debug.Log("gravity:" + gravity);

        // adjust gravity scale of player based on gravity required for jumpHeight & jumpTime
        rbody.gravityScale = gravity / Physics2D.gravity.y;

        // calculate jump velocity req'd for jumpHeight & jumpTime
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    //if the player dies or something odd happens just making sure the bullet returns to normal 
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
        Messenger<string>.AddListener(GameEvent.KEY_PICKUP, this.foundKey);
        Messenger.AddListener(GameEvent.TELEPORT, this.teleportPlayer);
      
    }


    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_HEALTH, this.OnPickupHealth);
        Messenger<string>.RemoveListener(GameEvent.POWER_UP, this.powerupChoice);
        Messenger<string>.RemoveListener(GameEvent.KEY_PICKUP, this.foundKey);
        Messenger.RemoveListener(GameEvent.TELEPORT, this.teleportPlayer);
       
    }


    //used for teleporting the player to the end of the game 
    private void teleportPlayer()
    {
        transform.position = fakeHouseSpawn.position;
        AudioSource source = gameObject.GetComponent<AudioSource>();
        source.clip = gameWinMusic;
        source.PlayOneShot(gameWinSound);
        source.Play();
    }

    //check for seeing if they player has found all they keys 
    public void foundKey(string keyName)
    {
        if(keyName == "SpikeKey")
        {
            hasSpikeKey = true;
        }
        else if (keyName == "MazeKey")
        {
            hasMazeKey = true;
        }
        else if (keyName == "EnemyKey")
        {
            hasEnemyKey = true;
        }
    }

    //coroutine for giving the player a powerup
    IEnumerator powerUpTimer(string powerup)
    {
        //give player stronger bullets
        if (powerup == "Apple") { 
            SpriteRenderer sprite = bullet.GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
            yield return new WaitForSeconds(15);
            sprite.color = Color.yellow; 
        }
        // give player bigger bullets
        else if (powerup == "Melon")
        {
            Transform sprite = bullet.GetComponent<Transform>();
            sprite.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(15);
            sprite.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        // make the player moon jump
        else if (powerup == "Cherry")
        {
            rbody.gravityScale /= 1.5f;
              yield return new WaitForSeconds(10);
            rbody.gravityScale *= 1.5f;
        }
        //make the player fast 
        else if (powerup == "Pineapple")
        {
            moveSpeed *= 1.5f;
             yield return new WaitForSeconds(10);
            moveSpeed /= 1.5f; 
        }
        // add an extra life to the player 
        else if (powerup == "ExtraLife")
        {
           if(playerLives != maxLives)
            {
                playerLives++;
                Messenger<float>.Broadcast(GameEvent.LIVES_CHANGED, ((float)playerLives / (float)maxLives));
            }
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

        //checking for firing the bullets
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

    //player picks up health 
    public void OnPickupHealth(int healthAdded)
    {
        //Debug.Log("Entered the onPickpHealth");
        currentHealth += healthAdded;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        float healthPercent = ((float)currentHealth) / (float)maxHealth;
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, healthPercent);
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
            audioSrc.PlayOneShot(jumpSound);
            anim.SetTrigger("jump");    // notify animator
        }else if (jumpsAvailable == 0)
        {
            audioSrc.PlayOneShot(jumpSound);
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

    //for firing the players bullets
    void Fire()
    {
            GameObject go = Instantiate(bullet, bulletSpawnPt.position, bullet.transform.rotation);
        if (facingRight)
        {
            audioSrc.PlayOneShot(bulletSound);
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200 * 5);
        }
        else
        {
            audioSrc.PlayOneShot(bulletSound);
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200 * 5);
        }
           
        destoryBall(go);
    }

    IEnumerator OnInvulnerable()
{
    isInvulnerable  = true;
   

    yield return new WaitForSeconds(1f); //how long player invulnerable

   
    isInvulnerable  = false;
}

    //player has been hit by something
    public void Hit()
    {
      
        if(!isInvulnerable)
        { 
        currentHealth -= 1;
            anim.SetTrigger("ouch");
            audioSrc.PlayOneShot(damageTakenSound);
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, ((float)currentHealth / (float)maxHealth));
        //Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0 && playerLives == 1)
        {
            audioSrc.PlayOneShot(gameOverSound);
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        } else if (currentHealth <= 0 && playerLives !=0)
        {
            playerLives--;
            currentHealth = maxHealth;
            transform.position = respawnPt.position;
            Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, ((float)currentHealth / (float)maxHealth));
            Messenger<float>.Broadcast(GameEvent.LIVES_CHANGED, ((float)playerLives / (float)maxLives));
        }
        StartCoroutine(OnInvulnerable());
        }
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "respawn")
        {
            respawnPt = collision.transform;
        }

        if (collision.gameObject.tag == "Bounds" && currentHealth != 0)
        {
            
            Hit();
            audioSrc.PlayOneShot(fallOffMapSound);
            transform.position = respawnPt.position;
        }
        else if (collision.gameObject.tag == "Bounds" && currentHealth == 0 && playerLives == 0)
        {
            // end game screen with restart and try again.
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
        else if (collision.gameObject.tag == "Door"
            && hasMazeKey == true 
            && hasEnemyKey == true 
            && hasSpikeKey == true)
        {
            Debug.Log("inside Door collision trigger");
            // Let the door know it can open 
            Messenger.Broadcast(GameEvent.DOOR_OPEN);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FatBird" || 
            collision.gameObject.tag == "AngryPig" ||
            collision.gameObject.tag == "Ghost" ||
             collision.gameObject.tag == "Slime" ||
             collision.gameObject.tag == "Tree" ||
            collision.gameObject.tag == "Projectile"
            && currentHealth != 0)
        {
            Hit();
        }
        else if (collision.gameObject.tag == "Spike")
        {
            Hit();
            audioSrc.PlayOneShot(spikeTrapSound);
            transform.position = respawnPt.position;
        }
    }
}
