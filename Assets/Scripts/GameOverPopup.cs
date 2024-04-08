using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private UIController uiController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void Awake()
    //{
    //    Messenger.AddListener(GameEvent.PLAYER_DEAD, this.Open);
    //}
    //private void OnDestroy()
    //{
    //    Messenger.RemoveListener(GameEvent.PLAYER_DEAD, this.Open);
    //}

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void OnExitGameButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }

    public void OnRestartGame()
    { 
        Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
        Debug.Log("Restart Game");
    }

}
