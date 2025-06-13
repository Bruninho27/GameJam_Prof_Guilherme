using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public int pontos = 10; // Pontua��o espec�fica para esse obst�culo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_move player = collision.GetComponent<Player_move>();


        if (player != null)
        {
            if (player.EstaAcelerandoOuAtacando())
            {
                // Ganha pontos e destr�i o obst�culo
                player.AdicionarPontos(pontos);
                Destroy(gameObject,2f); // Ou chamar uma anima��o de destrui��o
            }
            else
            {
                // Leva dano e fica lento
                player.ReceberColisaoComObstaculo();
            }
        }
    }

}
