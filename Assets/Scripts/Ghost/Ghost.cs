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

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }


    public float GetDistanceFromPlayer()
    {
        //Get the distance(in units) from the enemy to the player
        return Vector3.Distance(transform.position, Player.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootEvent()
    {
        // spawn a projectile using the spawnPoint
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPt.position, projectileSpawnPt.rotation);
        // move it forward
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
    }
}
