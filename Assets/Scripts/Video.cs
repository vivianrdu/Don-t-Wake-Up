using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    VideoPlayer vid;
    // Start is called before the first frame update
    void Start()
    {
        vid = GetComponent<VideoPlayer>();
        vid.Play();
        vid.loopPointReached += MoveScene;

    }

    void Update()
    {

    }

    void MoveScene (UnityEngine.Video.VideoPlayer vp)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "1.DarkSceneAnimated")
        {
            SceneManager.LoadScene("2.WaterScene");
        }
        else if (currentScene == "2.WaterSceneAnimated")
        {
            SceneManager.LoadScene("3.PeopleScene");
        }
        else if (currentScene == "3.PeopleSceneAnimated")
        {
            SceneManager.LoadScene("4.End");
        }
        else
        {
            SceneManager.LoadScene("0.StartMenu");
        }
    }
}
