using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && this.gameObject.tag == "Apple" 
            || other.gameObject.tag == "Player" &&  this.gameObject.tag == "Pineapple"
            || other.gameObject.tag == "Player" &&  this.gameObject.tag == "Melon"
            || other.gameObject.tag == "Player" &&  this.gameObject.tag == "Cherry")
        {
            Messenger<string>.Broadcast(GameEvent.POWER_UP, this.gameObject.tag);
            anim.SetTrigger("collected");
            StartCoroutine(waitBeforeDestroy(this.gameObject));

        }
       
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{

      
    //    if(collision.gameObject.tag == "Melon")
    //    {

    //        Messenger<string>.Broadcast(GameEvent.POWER_UP, this.gameObject.tag);
    //        anim.SetTrigger("collected");
    //        StartCoroutine(waitBeforeDestroy(this.gameObject));
    //    }


    //}
}
