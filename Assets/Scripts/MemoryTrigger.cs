using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if ((Input.GetKeyDown(KeyCode.E)) && (distFromPlayer <= radius))
        {
            GameObject gm = GameObject.FindWithTag("GameController");
            gm.GetComponent<GameManager>().DarkSceneCutscene();
        }
    }
}
