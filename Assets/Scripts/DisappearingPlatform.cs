using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{

 
    [SerializeField] GameObject platform;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(deactivate());
        }
    }

    IEnumerator activate()
    {

        //Transform position = platform.transform;
        yield return new WaitForSeconds(1);

        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(1);//wait x amount of seconds
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
       

        Debug.Log("Exited Deactivate");
    }

    IEnumerator deactivate()
    {
       
        yield return new WaitForSeconds(2);//wait x amount of seconds
       spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2);//wait x amount of seconds
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    
    }
}
