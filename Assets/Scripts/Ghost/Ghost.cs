using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float IdleTime { get; private set; } = 3.0f;         // time to spend in idle state
    public float ChaseRange { get; private set; } = 10.0f;      // when player is closer than this, chase
    public float AttackRange { get; private set; } = 7.0f;      // when player is closer than this, attack
    public float AttackRangeStop { get; private set; } = 20.0f; // when player is farther than this, chase

    public GameObject Player { get; private set; }

    [SerializeField] private GameObject projectilePrefab;       // for creating "bullets"
    [SerializeField] public Transform projectileSpawnPt;        // spawn point for bullets    
    [SerializeField] private AudioClip hitMarkerSound;
    [SerializeField] private AudioSource audioSrc;
    bool facingRight = false;

    Vector3 lastKnownPos;
    Rigidbody2D rb;

    int ghostHealth = 5;
    int ghostMaxHealth = 5;
    int ghostPointWorth = 500;
    [SerializeField] FloatingHealthBar healthBar;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        healthBar.UpdateHealthBar(ghostHealth, ghostMaxHealth);
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    void Flip()
    {
        // flip the direction the player is facing
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180);
    }

    public float GetDistanceFromPlayer()
    {
        //Get the distance(in units) from the enemy to the player
        return Vector3.Distance(transform.position, Player.transform.position);
    }
    // Update is called once per frame
    void Update()
    {

        // headed right && not facing right
        if (transform.position.x > lastKnownPos.x && !facingRight)
        {
            Flip();
        }
        // headed left && facing right
        else if (transform.position.x < lastKnownPos.x && facingRight)
        {
            Flip();
        }

        lastKnownPos = transform.position; 
    }

    public void MoveTowards(Vector3 target)
    {
        rb.MovePosition(target);
    }
    
    public void ShootEvent()
    {
        //Debug.Log("shooting");
        // spawn a projectile using the spawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPt.position, projectileSpawnPt.rotation);
        // move it forward
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //Determine which way the bullet should be shot out at
        if(facingRight)
        {
            rb.AddForce(Vector2.right * 200 * 5);
        }
        else
        {
            rb.AddForce(Vector2.left * 200 * 5);
        }
       
    }

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(gameObject.tag);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet") {
            audioSrc.PlayOneShot(hitMarkerSound);
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Animator anim = gameObject.GetComponent<Animator>();
            if (sprite.color == Color.red)
            {
                ghostHealth -= 2;
                healthBar.UpdateHealthBar(ghostHealth, ghostMaxHealth);
                anim.SetTrigger("ouch");
                if (ghostHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, ghostPointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
            else if (sprite.color == Color.yellow)
            {
                ghostHealth--;
                healthBar.UpdateHealthBar(ghostHealth, ghostMaxHealth);
                anim.SetTrigger("ouch");
                if (ghostHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, ghostPointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }

        }
    }
}
