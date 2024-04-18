using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;       // for creating "bullets"
    [SerializeField] public Transform projectileSpawnPt;        // spawn point for bullets
    [SerializeField] private AudioClip hitMarkerSound;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private Animator anim;
    [SerializeField] FloatingHealthBar healthBar;
    // Start is called before the first frame update

    public bool facingRight;

    int treeHealth = 6;
    int treeMaxHealth = 6;
    int treePointWorth = 350;

    float attackTimer;
    float attackTimeThreshold = 2.0f;
    void Start()
    {
        healthBar.UpdateHealthBar(treeHealth, treeMaxHealth);
        if (facingRight)
        {
            transform.Rotate(Vector3.up, 180);
        }
        else
        {
            facingRight = false;
        }
        attackTimer = 0;
      
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
       
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimeThreshold)
        {
            anim.SetTrigger("attack");
            ShootEvent();
            
            attackTimer = 0;
        }
    }

    public void ShootEvent()
    {
        // spawn a projectile using the spawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPt.position, projectileSpawnPt.rotation);
        // move it forward
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //Determine which way the bullet should be shot out at
        if (facingRight)
        {
            rb.AddForce(Vector2.right * 200 * 5);
        }
        else if (!facingRight)
        {
            rb.AddForce(Vector2.left * 200 * 5 );
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
        if (collision.gameObject.tag == "Bullet")
        {
            audioSrc.PlayOneShot(hitMarkerSound);
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Animator anim = gameObject.GetComponent<Animator>();
            if (sprite.color == Color.red)
            {
                treeHealth -= 2;
                healthBar.UpdateHealthBar(treeHealth, treeMaxHealth);
                anim.SetTrigger("ouch");
                if (treeHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, treePointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
            else if (sprite.color == Color.yellow)
            {
                treeHealth--;
                healthBar.UpdateHealthBar(treeHealth, treeMaxHealth);
                anim.SetTrigger("ouch");
                if (treeHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, treePointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }

        }
    }
}
