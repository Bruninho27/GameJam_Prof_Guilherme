using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perseguidorControler : MonoBehaviour
{

    public Transform player;
    public float distanciaAtras = 5f;
    public float velocidade = 2f;

    private bool ativo = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!ativo || player == null) return;

        Vector3 alvo = new Vector3(player.position.x - distanciaAtras, player.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, alvo, velocidade * Time.deltaTime);
    }
    public void Ativar()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x - distanciaAtras, player.position.y, player.position.z);
        }
        ativo = true;
    }
    public void Avancar()
    {
        // Se quiser que ele se aproxime mais rápido a cada dano
        distanciaAtras = Mathf.Max(1f, distanciaAtras - 1f);
    }

}
