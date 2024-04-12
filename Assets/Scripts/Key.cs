using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;
    [SerializeField] AudioSource src;
    [SerializeField] private AudioClip keyCollectSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Player" )
        {

            Messenger<string>.Broadcast(GameEvent.KEY_PICKUP, this.gameObject.tag);
            Messenger.Broadcast(GameEvent.KEY_COLLECTED);
            anim.SetTrigger("collected");
            StartCoroutine(waitBeforeDestroy(this.gameObject));
        }


    }

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        src.PlayOneShot(keyCollectSound);
        yield return new WaitForSeconds(1f);
        Debug.Log(gameObject.tag);
        Destroy(gameObject);
    }
}
