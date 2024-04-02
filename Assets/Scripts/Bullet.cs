using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //for now this works to kill enemies but i need to give them health and then do health checks 
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Destroy(gameObject);
        if (collision.gameObject.tag == "Enemy")
        {
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
