using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryTrigger : MonoBehaviour
{
    #region Trigger_variables
    private CircleCollider2D coll;
    #endregion

    #region Other_variables
    private float radius;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        radius = coll.radius;
    }

    void Update()
    {
        float distFromPlayer = Vector2.Distance(player.position, transform.position);
        string currentScene = SceneManager.GetActiveScene().name;

        if ((Input.GetKeyDown(KeyCode.E)) && (distFromPlayer <= radius))
        {
            GameObject gm = GameObject.FindWithTag("GameController");
            if (currentScene == "0.Tutorial")
            {
                gm.GetComponent<GameManager>().DarkScene();
            }
            else if (currentScene == "1.DarkScene")
            {
                gm.GetComponent<GameManager>().DarkSceneCutscene();
            }
            else if (currentScene == "2.WaterScene")
            {
                gm.GetComponent<GameManager>().WaterSceneCutscene();
            }
            else if (currentScene == "3.PeopleScene")
            {
                gm.GetComponent<GameManager>().PeopleSceneCutscene();
            }
        }
    }
}
