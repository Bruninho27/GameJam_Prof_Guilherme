using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_AtkPlay : MonoBehaviour
{
    private Player_move player;

    void Start()
    {
        player = GetComponentInParent<Player_move>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstaculo obstaculo = collision.GetComponent<Obstaculo>();
        if (obstaculo != null)
        {
            if (player != null && player.EstaAcelerandoOuAtacando())
            {
                player.AdicionarPontos(obstaculo.pontos);
                Destroy(obstaculo.gameObject,2f);
            }
            else if (player != null)
            {
                player.ReceberColisaoComObstaculo();
            }
        }
    }

}
