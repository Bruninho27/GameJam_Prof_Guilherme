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

    // Update is called once per frame
    void Update()
    {
        moveAnim();


    }
    void moveAnim()
    {
        if (PL.Movex > 0 || PL.Movex < 0 )
        {
            animPlayer.SetInteger("transition", 1); //animaçao de andar
        }

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
