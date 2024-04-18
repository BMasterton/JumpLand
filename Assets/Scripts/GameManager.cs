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
    private int keyTotal = 0;

   
  
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
        Messenger.AddListener(GameEvent.GAME_WIN, GameWinPopup);
        Messenger.AddListener(GameEvent.KEY_COLLECTED, OnKeyCollected);


    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, GameOverPopUp);
        Messenger.RemoveListener(GameEvent.GAME_WIN, GameWinPopup);
        Messenger.RemoveListener(GameEvent.KEY_COLLECTED, OnKeyCollected);

    }

    private void OnEnemyDead(int pointWorth)
    {
        score += pointWorth;
        ui.UpdateScore(score);
    }

    private void OnKeyCollected()
    {
        keyTotal++;
        ui.UpdateKey(keyTotal);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

 private void GameWinPopup()
    {
        
        ui.OpenGameWinPopup(score);
    }

    private void GameOverPopUp()
    {
        ui.OpenGameOverPopup();
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
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


        Debug.Log("Exited Deactivate");
    }

    IEnumerator deactivate()
    {

        Transform position = platform.transform;
        Debug.Log("Entered deactivate");
        yield return new WaitForSeconds(2);//wait x amount of seconds
        platform.SetActive(false);

        StartCoroutine(activate());
    }
}
