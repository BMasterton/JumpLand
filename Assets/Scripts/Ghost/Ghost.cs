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
    private float projectileForce = 35f;
    bool facingRight = false;

    Vector3 lastKnownPos;
    Rigidbody2D rb;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Flip()
    {
        Debug.Log("flip");
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

        // float currentValue = rb.velocity.x;
        //if (rb.velocity.x > 0 && !facingRight)
        //{
        //    Flip();
        //}
        //else if (rb.velocity.x < 0 && facingRight )
        //{
        //    Flip();
        //}

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

        // pastValue = currentValue;
        //// float currentValue = rb.velocity.x;
        // if (rb.velocity.x - pastValue < 0)
        // {
        //     Flip();
        // }
        // else if(rb.velocity.x - pastValue > 0) { 
        //     Flip();
        // }

        // pastValue = currentValue;
        lastKnownPos = transform.position; 
    }

    public void MoveTowards(Vector3 target)
    {
        rb.MovePosition(target);
    }
    
    public void ShootEvent()
    {
        Debug.Log("shooting");
        // spawn a projectile using the spawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPt.position, projectileSpawnPt.rotation);
        // move it forward
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if(facingRight)
        {
            rb.AddForce(Vector2.right * 200 * 5);
        }
        else
        {
            rb.AddForce(Vector2.left * 200 * 5);
        }
       
    }
}
