using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Enemy
{
    [Header("Variables From Child Class")]
    #region Player_Variables
    public Enemy_People[] nearbyEnemies;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Startup();
        anim.SetBool("playerDetected", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerposition == null || player_in_Game == null)
        {
            anim.SetBool("playerDetected", false);
            return;
        }
        //detected player in line of sight
        else
        {
            if (player_in_Game.isHidden)
            {
                anim.SetBool("playerDetected", false);
            }
            else
            {
                if (player_in_Game.isRunning)
                {
                    anim.SetBool("playerDetected", true);
                    WarnNearbyEnemies();
                }
            }
        }
    }

    void WarnNearbyEnemies()
    {
        for (int i = 0; i < nearbyEnemies.Length; ++i)
        {

            Enemy_People person = nearbyEnemies[i];
            person.change_orientation_to_painting(transform);
        }
    }
}
