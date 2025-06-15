using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projetil : MonoBehaviour
{
    public float speed = 5f;
    public int dano = 20;

    private Vector3 direction;
   [SerializeField] private float vidaMaxima = 5f; // Tempo para o projétil sumir
    private float vidaAtual;
    // Start is called before the first frame update
    void Start()
    {
        vidaAtual = vidaMaxima;
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += direction * speed * Time.deltaTime;

        vidaAtual -= Time.deltaTime;
        if (vidaAtual <= 0)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_atk player = other.GetComponent<Player_atk>();
            if (player != null)
            {
                player.receberdano(dano);
            }
            Destroy(this.gameObject);
        }
        
    }
}

