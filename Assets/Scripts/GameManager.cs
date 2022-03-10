using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    #region Unity_functions

    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Scene_transition
    
    //Loads the StartMenu
    public void StartMenu()
    {
        SceneManager.LoadScene("0.StartMenu");
        
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("0.Tutorial");
    }

    public void DarkScene()
    {
        SceneManager.LoadScene("1.DarkScene");
    }

    public void DarkSceneCutscene()
    {
        SceneManager.LoadScene("1.DarkSceneAnimated");
    }

    public void WaterScene()
    {
        SceneManager.LoadScene("2.WaterScene");
    }

    public void WaterSceneCutscene()
    {
        SceneManager.LoadScene("2.WaterSceneAnimated");
    }

    public void PeopleScene()
    {
        SceneManager.LoadScene("3.PeopleScene");
    }

    public void PeopleSceneCutscene()
    {
        SceneManager.LoadScene("3.PeopleSceneAnimated");
    }
    #endregion
}
