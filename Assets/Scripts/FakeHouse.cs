using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHouse : MonoBehaviour
{
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
            //StartCoroutine(waitBeforeCompleteScreen());
            Messenger.Broadcast(GameEvent.GAME_WIN);
        }




    }
    //IEnumerator waitBeforeCompleteScreen()
    //{
    //    //src.PlayOneShot(teleportSound);
    //    yield return new WaitForSeconds(1f);


    //}
}
