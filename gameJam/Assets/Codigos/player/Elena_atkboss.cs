using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elena_atkboss : MonoBehaviour
{
    public int damage = 25;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entrou na hitbox: " + other.name);
        if (other.CompareTag("Boss"))
        {
              Debug.Log("Acertou o Boss!");
            Boss Bos = other.GetComponentInParent<Boss>();
            if (Bos != null)
            {
                Bos.LevarDano(damage); // Dano fixo ou variável
            }
            else
            {
                Debug.LogWarning("BossController não encontrado no objeto ou nos pais de: " + other.name);
            }
        }
    }

}
