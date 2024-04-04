using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject platform;
    [SerializeField] PlayerController player;

    private int playerScore = 0;
  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void ChangeToLevelTwo()
    {
        //change level stuff here 
        //change scene 
        //load new one and stuffs, redo timers, and all that 
    }

    public void ChangeToLevelThree()
    {
        //change level stuff here 
        //change scene 
        //load new one and stuffs, redo timers, and all that 
    }

    public void ChangeToLevelFour()
    {
        //change level stuff here 
        //change scene 
        //load new one and stuffs, redo timers, and all that 
    }

    public void EndGame()
    {
        //end game stuff here 
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


        Transform position = platform.transform;
        yield return new WaitForSeconds(2);
        platform.SetActive(true);
        //_ = Instantiate(platform) as GameObject;
        //platform.transform.position = position.position;

        Debug.Log("Exited Deactivate");
    }

    IEnumerator deactivate()
    {

        Transform position = platform.transform;
        Debug.Log("Entered deactivate");
        yield return new WaitForSeconds(2);//wait x amount of seconds
        platform.SetActive(false);
        //Destroy(platform);

        StartCoroutine(activate());
    }
}
