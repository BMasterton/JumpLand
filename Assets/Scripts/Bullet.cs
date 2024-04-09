using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator destoryBullet(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Untagged" 
            || collision.gameObject.tag == "platform" || collision.gameObject.tag == "FatBird"
            || collision.gameObject.tag == "AngryPig" || collision.gameObject.tag == "Ghost"
            || collision.gameObject.tag == "Slime")
        {
            if (collision.gameObject.tag == "Slime"
           || collision.gameObject.tag == "AngryPig"
           || collision.gameObject.tag == "Ghost"
           || collision.gameObject.tag == "FatBird")
            {
                GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(impact, 1);     // destroy the explosion after 2 seconds

                 // destroy the projectile
            }
            //Destroy(collision.gameObject);
           Destroy(gameObject);
        }

       
        yield return new WaitForSeconds(0.55f);
        //might need to make it so they get destroyed when touching anything
        Destroy(gameObject);
    }

        //for now this works to kill enemies but i need to give them health and then do health checks 
        private void OnCollisionEnter2D(Collision2D collision)
        {

        StartCoroutine(destoryBullet(collision));
       
       
    }
}
