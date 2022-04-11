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
        Debug.Log("Load DarkScene");
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
        Enemy[] enemies_in_Scene;
        Nightlight[] nightlights;
        Crate[] crates;
        //Scene scene = SceneManager.GetActiveScene();
        enemies_in_Scene = FindObjectsOfType<Enemy>();
        nightlights = FindObjectsOfType<Nightlight>();
        crates = FindObjectsOfType<Crate>();

        // iterate root objects and do something
        for (int i = 0; i < enemies_in_Scene.Length; ++i)
        {
            //Make an Enemy Class then let each class inherit main functions from Parent
            if(enemies_in_Scene[i] is Enemy_Dark) { 
            Enemy_Dark enemy = (Enemy_Dark) enemies_in_Scene[i];
                enemy.Reset_position();
            }else if(enemies_in_Scene[i] is Enemy_Large) {
                Enemy_Large enemy = (Enemy_Large)enemies_in_Scene[i];
                enemy.Reset_position();
            }
            else if (enemies_in_Scene[i] is Enemy_Sleeper)
            {
                Enemy_Sleeper enemy = (Enemy_Sleeper)enemies_in_Scene[i];
                enemy.Reset_position();
            }

        }

        for (int i = 0; i < crates.Length;i++)
        {
            Crate crate = crates[i];
            crate.Reset_position();
        }

        for (int i = 0; i < nightlights.Length; ++i)
        {
            //Make an Enemy Class then let each class inherit main functions from Parent

            Nightlight nightlight = nightlights[i];
            nightlight.Reset_position();
        }
    }


}
