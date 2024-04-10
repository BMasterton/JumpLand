using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource src;
    [SerializeField] private AudioClip teleportSound;
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
            StartCoroutine(waitBeforeDestroy(this.gameObject));
            Messenger.Broadcast(GameEvent.TELEPORT);
        }


    }

    IEnumerator waitBeforeDestroy(GameObject gameObject)
    {
        src.PlayOneShot(teleportSound);
        yield return new WaitForSeconds(1f);
      
    }
}
