using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;
    int maxHealth = 3;
    int health = 3;
    int enemyPointWorth = 100;

    bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
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
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            if(sprite.color == Color.red)
            {
                health -= 2;
                anim.SetTrigger("ouch");   
                if (health <= 0)
                {
                    //for every enemy you make add this and have different point values 
                    Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, enemyPointWorth);
                    anim.SetTrigger("dead");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
            else if (sprite.color == Color.yellow) { 
                health--;
            anim.SetTrigger("ouch");  
            if ( health <= 0)
            {
                //for every enemy you make add this and have different point values 
                Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, enemyPointWorth);
                    anim.SetTrigger("dead");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
           
            //how to get to stop aniimation 
            //hit = false;
            //anim.SetBool("hit", hit) ;

        }
    }
}
