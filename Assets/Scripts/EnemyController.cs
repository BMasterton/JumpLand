using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] Animator anim;
    [SerializeField] private AudioClip hitMarkerSound;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] FloatingHealthBar healthBar;
    private bool pigFacingRight = true;    // true if facing right
    int birdHealth = 3;
    int birdMaxHealth = 3;
    int pigHealth = 5;
   int pigMaxHealth = 5;
    int birdPointWorth = 100;
    int pigPointWorth = 250;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.UpdateHealthBar(pigHealth, pigMaxHealth);
        healthBar.UpdateHealthBar(birdHealth, birdMaxHealth);
    }
    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log(gameObject.tag);
        Destroy(gameObject);
    }

    void Flip()
    {
        // flip the direction the player is facing
        pigFacingRight = !pigFacingRight;
        transform.Rotate(Vector3.up, 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioSrc.PlayOneShot(hitMarkerSound);
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            if(sprite.color == Color.red)
            {
                if(this.gameObject.tag == "FatBird")
                {
                   
                    birdHealth -= 2;
                    healthBar.UpdateHealthBar(birdHealth, birdMaxHealth);
                }
                else if (this.gameObject.tag == "AngryPig")
                {
                    pigHealth -= 2;
                    healthBar.UpdateHealthBar(pigHealth, pigMaxHealth);
                }
                anim.SetTrigger("ouch");   
                if (birdHealth <= 0 || pigHealth <=0)
                {
                    anim.SetTrigger("dead");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                    //for every enemy you make add this and have different point values 
                    if (this.gameObject.tag == "FatBird")
                    {
                         Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, birdPointWorth);
                    }
                    else if (this.gameObject.tag == "AngryPig")
                    {
                        Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, pigPointWorth);
                    }
                   
                    anim.SetTrigger("dead");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                }
            }
            else if (sprite.color == Color.yellow) {
                if (this.gameObject.tag == "FatBird")
                {
                    birdHealth--;
                    healthBar.UpdateHealthBar(birdHealth, birdMaxHealth);
                }
                else if (this.gameObject.tag == "AngryPig")
                {
                    pigHealth--;
                    healthBar.UpdateHealthBar(pigHealth, pigMaxHealth);
                }
                anim.SetTrigger("ouch");  
            if ( birdHealth <= 0 || pigHealth <= 0)
            {
                    //for every enemy you make add this and have different point values 
                    anim.SetTrigger("dead");
                    StartCoroutine(waitBeforeDestroy(this.gameObject));
                    if (this.gameObject.tag == "FatBird")
                    {
                        Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, birdPointWorth);
                    }
                    else if (this.gameObject.tag == "AngryPig")
                    {
                        Messenger<int>.Broadcast(GameEvent.ENEMY_DEAD, pigPointWorth);
                    }
                    
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
