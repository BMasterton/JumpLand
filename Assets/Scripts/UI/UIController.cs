using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI keyValue;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image livesBar;
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private GameCompletedPopup gameCompletedPopup;
    void Start()
    {
        UpdateHealth(1.0f);
        OnHealthChanged(1.0f);
    }


    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<float>.AddListener(GameEvent.LIVES_CHANGED, OnLivesChanged);

    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
        Messenger<float>.RemoveListener(GameEvent.LIVES_CHANGED, OnLivesChanged);

    }
    public void OpenGameOverPopup()
    {
        SetGameActive(false);
        gameOverPopup.Open();
    }


    public void OnHealthChanged(float healthPercentage)
    {
        UpdateHealth(healthPercentage);
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    public void OnLivesChanged(float livePercentage)
    {
        UpdateLives(livePercentage);
    }

    public void UpdateLives(float life)
    {
        livesBar.fillAmount = life;
    }
    public void UpdateHealth(float health)
    {
        healthBar.fillAmount = health;
    }


    public void UpdateScore(int newScore)
    {
        scoreValue.text = newScore.ToString() ;
    }
    public void UpdateKey(int newKey)
    {
        keyValue.text = newKey.ToString() + " / 3";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsPopup.IsActive())
        {
            SetGameActive(false);
            optionsPopup.Open();
        }
    }

    private void OnPopupClosed()
    {
            SetGameActive(true);
    }

    private void OnPopupOpened()
    {
            SetGameActive(false);
    }


    public void OpenGameWinPopup(int score)
    {
        SetGameActive(false);
        gameCompletedPopup.Open(score);
    }
    public void SetGameActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 1; // unpause the game
        }
        else
        {
            Time.timeScale = 0; // pause the game
        }
    }
}
