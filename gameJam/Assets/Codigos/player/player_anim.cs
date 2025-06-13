using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_anim : MonoBehaviour
{
    private Player_move PL;
    private Animator animPlayer;
    // Start is called before the first frame update
    void Start()
    {
        animPlayer = GetComponent<Animator>();
        PL = GetComponent<Player_move>();

    }

   

    public void UpdateStates(bool isAccelerating)
    {
        float speed = PL.GetComponent<Rigidbody2D>().velocity.x;

      

        // Correndo rápido
        if (isAccelerating && speed > PL.speed + 0.1f)
        {
            animPlayer.SetInteger("transition", 2);
        }
        // Andando
        else if (speed > 0.1f)
        {
            animPlayer.SetInteger("transition", 1);
        }
        // Parado
        else
        {
            animPlayer.SetInteger("transition", 0);
        }
    }
   
    public void atk()
    {
        animPlayer.SetTrigger("atk");
    }



}
