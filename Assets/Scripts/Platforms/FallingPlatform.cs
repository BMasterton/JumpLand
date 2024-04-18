using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    //[SerializeField] PlayerController playerController;

    Vector2 startPos;
    float fallWaitTime = 1.0f;

   

    public bool respawns = true;
    // Start is called before the first frame update
    void Start()
    {
   
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           
            StartCoroutine(waitBeforeFall(fallWaitTime)); // not working for some reason 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "bound2" && respawns)
        {
            StartCoroutine(waitBeforeRespawn(fallWaitTime));
            //rb.isKinematic = true;
            //rb.velocity = new Vector3(0,0,0);
            //transform.position = startPos;
        }
    }

    IEnumerator waitBeforeFall(float waitTime)
    { 
        yield return new WaitForSeconds(waitTime);
        rb.isKinematic = false;
    }

    IEnumerator waitBeforeRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = startPos;
    }
}
