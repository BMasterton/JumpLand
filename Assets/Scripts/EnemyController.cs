using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;
    int maxHealth = 3;
    int health = 3;

    bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
           
            health--;
            anim.SetTrigger("ouch");  //not sure how to get the animation to work correctly and go back to idle 
            if ( health == 0)
            {
                Destroy(gameObject);
            }
            //how to get to stop aniimation 
            //hit = false;
            //anim.SetBool("hit", hit) ;

        }
    }
}
