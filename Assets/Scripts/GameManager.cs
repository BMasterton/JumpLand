using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject platform;
    [SerializeField] PlayerController player;
    [SerializeField] private UIController ui;
    private int score = 0;

   
  
    // Start is called before the first frame update
    void Start()
    {
        ui.UpdateScore(score);
    }


    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
       
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, GameOverPopUp);


    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
       
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, GameOverPopUp);

    }

    private void OnEnemyDead(int pointWorth)
    {
        score += pointWorth;
        ui.UpdateScore(score);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

 

    private void GameOverPopUp()
    {
        ui.OpenGameOverPopup();
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
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
