using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private int healthValue = 1;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {
            Messenger<int>.Broadcast(GameEvent.PICKUP_HEALTH, healthValue);
            anim.SetTrigger("collected");
            StartCoroutine(waitBeforeDestroy(this.gameObject));
        }


    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Entered the OnTriggerEnter");
    //        Messenger<int>.Broadcast(GameEvent.PICKUP_HEALTH, healthValue);
    //        Destroy(this.gameObject);

    //    }
    //}

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(gameObject.tag);
        Destroy(gameObject);
    }
}
