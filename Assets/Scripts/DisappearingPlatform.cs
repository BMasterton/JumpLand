using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{

    //// Variables to control timing
    //private bool playerOnPlatform = false;
    //private float timePlayerEntered;
    //private bool platformActive = true;

    //// Variables for timing intervals
    //public float disappearDelay = 2f; // Time it takes for the platform to disappear after player lands on it
    //public float appearDelay = 5f;    // Time it takes for the platform to reappear after disappearing

    //// Reference to the platform's SpriteRenderer
    //private SpriteRenderer spriteRenderer;
    //private BoxCollider2D boxCollider;

    //private void Start()
    //{
    //    // Get the SpriteRenderer component
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //    boxCollider = GetComponent<BoxCollider2D>();
    //}

    //private void Update()
    //{
    //    if (playerOnPlatform && platformActive)
    //    {
    //        // If the player is on the platform and it's still active
    //        if (Time.time - timePlayerEntered > disappearDelay)
    //        {
    //            // If enough time has passed, make the platform disappear
    //            spriteRenderer.enabled = false;
    //            boxCollider.enabled = false;
    //            platformActive = false;
    //            // Start timer for reappearing
    //            Invoke("MakePlatformAppear", appearDelay);
    //        }
    //    }
    //}

    //private void MakePlatformAppear()
    //{
    //    // Make the platform reappear
    //    spriteRenderer.enabled = true;
    //    boxCollider.enabled = true;
    //    platformActive = true;
    //    // Reset player on platform flag
    //    playerOnPlatform = false;
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // If player lands on the platform, set the playerOnPlatform flag and record the time
    //        playerOnPlatform = true;
    //        timePlayerEntered = Time.time;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // If player leaves the platform, reset the playerOnPlatform flag
    //        playerOnPlatform = false;
    //    }
    //}
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
        yield return new WaitForSeconds(2);

        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2);//wait x amount of seconds
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        //platform.SetActive(true);
        //_ = Instantiate(platform) as GameObject;
        //platform.transform.position = position.position;

        Debug.Log("Exited Deactivate");
    }

    IEnumerator deactivate()
    {
       
        //Transform position = platform.transform;
        //Debug.Log("Entered deactivate");
        yield return new WaitForSeconds(2);//wait x amount of seconds
       spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2);//wait x amount of seconds
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        //platform.SetActive(false);
        //Destroy(platform);

        //StartCoroutine(activate());
    }
}
