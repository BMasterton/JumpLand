using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using BasePopup;

public class OptionsPopup : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    //[SerializeField] private SettingsPopup settingsPopup;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
    public void OnReturnToGameButton()
    {
        Debug.Log("return to game");
        uiController.SetGameActive(true);
        Close();
    }
}
