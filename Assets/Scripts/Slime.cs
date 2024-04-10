using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [SerializeField] private GameObject projectilePrefab;       // for creating "bullets"
    [SerializeField] public Transform projectileSpawnPt;        // spawn point for bullets
    [SerializeField] private AudioClip hitMarkerSound;
    [SerializeField] private AudioSource audioSrc;
    bool facingRight = false;

    int slimeHealth = 4;
    int slimePointWorth = 350;

    float attackTimer;
    float attackTimeThreshold = 3.0f;


    void Flip()
    {
        // flip the direction the player is facing
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180);
    }
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimeThreshold)
        {
            ShootEvent();
            attackTimer = 0;
        }
    }

    public void ShootEvent()
    {
        //Debug.Log("shooting");
        // spawn a projectile using the spawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPt.position, projectileSpawnPt.rotation);
        // move it forward
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //Determine which way the bullet should be shot out at
        if (facingRight)
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
        if (collision.gameObject.tag == "Bullet")
        {
            audioSrc.PlayOneShot(hitMarkerSound);
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Animator anim = gameObject.GetComponent<Animator>();
            if (sprite.color == Color.red)
            {
                slimeHealth -= 2;
                anim.SetTrigger("ouch");
                if (slimeHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, slimePointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
            else if (sprite.color == Color.yellow)
            {
                slimeHealth--;
                anim.SetTrigger("ouch");
                if (slimeHealth <= 0)
                {
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, slimePointWorth);
                    anim.SetTrigger("die");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }

        }
    }
}
