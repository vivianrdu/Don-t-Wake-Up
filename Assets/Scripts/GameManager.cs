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

    public void Reset_current_scene()
    {
        Enemy_Dark[] enemies_in_Scene;
        //Scene scene = SceneManager.GetActiveScene();
        enemies_in_Scene = FindObjectsOfType<Enemy_Dark>();

        // iterate root objects and do something
        for (int i = 0; i < enemies_in_Scene.Length; ++i)
        {
            //Make an Enemy Class then let each class inherit main functions from Parent
            
            Enemy_Dark enemy_Dark = enemies_in_Scene[i];
            enemy_Dark.Reset_position();
        }
    }


}
