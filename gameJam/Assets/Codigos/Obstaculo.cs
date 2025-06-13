using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public int pontos = 10; // Pontuação específica para esse obstáculo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_move player = collision.GetComponent<Player_move>();


        if (player != null)
        {
            if (player.EstaAcelerandoOuAtacando())
            {
                // Ganha pontos e destrói o obstáculo
                player.AdicionarPontos(pontos);
                Destroy(gameObject,2f); // Ou chamar uma animação de destruição
            }
            else
            {
                // Leva dano e fica lento
                player.ReceberColisaoComObstaculo();
            }
        }
    }

}
