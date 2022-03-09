using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    #region Unity_functions

    private void Awake()
    {
        if (Instance == null){
            Instance = this;
        }else if (Instance != this) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Scene_transition
    
    //Loads the StartMenu
    public void StartMenu()
    {
        SceneManager.LoadScene("0.StartMenu");
        
    }

    public void DarkScene()
    {
        SceneManager.LoadScene("1.DarkScene");
    }

    #endregion
}
