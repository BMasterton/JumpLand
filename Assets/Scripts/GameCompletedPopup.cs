using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompletedPopup : MonoBehaviour
{
    // Start is called before the first frame update

    int score = 0;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private UIController uiController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        scoreValue.text = "Score: " + score.ToString();
    }

    public void Open(int score)
    {
        gameObject.SetActive(true);
        SetScore(score);
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
        uiController.SetGameActive(true);

        Debug.Log("Restart Game");
    }
}
