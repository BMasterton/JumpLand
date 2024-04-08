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
    [SerializeField] private Image healthBar;
    [SerializeField] private OptionsPopup optionsPopup;
    [SerializeField] private GameOverPopup gameOverPopup;
    void Start()
    {
        UpdateHealth(1.0f);
        OnHealthChanged(1.0f);
    }


    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);
       
    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.HEALTH_CHANGED, OnHealthChanged);

    }
    public void OpenGameOverPopup()
    {
        gameOverPopup.Open();
    }


    public void OnHealthChanged(float healthPercentage)
    {
        //Debug.Log("OnHealthChanged");
        //Debug.Log(healthPercentage);
        UpdateHealth(healthPercentage);
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
    public void UpdateHealth(float health)
    {
        healthBar.fillAmount = health;
    }


    public void UpdateScore(int newScore)
    {
        scoreValue.text = newScore.ToString();
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
