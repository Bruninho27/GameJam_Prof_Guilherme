using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{

    // Variáveis
    [Header("Variáveis para mover")]
    private float movex; // Movimento horizontal do jogador
    private Rigidbody2D RB; // Referência ao Rigidbody2D para física
    [SerializeField] private float speed; // Velocidade de movimento do jogador
    bool isfacingRigth = true; // Indica se o jogador está virado para a direita


    [Header("respaw")]
    private Vector2 Checkpoint;

    [Header("Vida")]
    public int maxHealth = 7; // Vida máxima do jogador
    public int vida; // Vida atual do jogador
    public bool intangivel; // Indica se o jogador é intangível
    private SpriteRenderer spriteRenderer;
    private bool Hit;

    public float Movex { get => movex; set => movex = value; }

    void Start()
    {
        vida = maxHealth; // Inicializa a vida com o valor máximo
        RB = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
        Checkpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movex = Input.GetAxisRaw("Horizontal"); // Captura a entrada horizontal do jogador

        if (movex<0f||movex>0f)
        {
            OnMove();
            flip();
        }
    }
    void OnMove()
    {
        // Atualiza a velocidade horizontal do jogador
        RB.velocity = new Vector2(speed * movex, RB.velocity.y);
    }
    void flip()
    {

        if (isfacingRigth && movex < 0f || !isfacingRigth && movex > 0f)
        {
            isfacingRigth = !isfacingRigth;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }

    }
    IEnumerator damageplayer()
    {

        for (float i = 0f; i < 1f; i += 0.1f)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        intangivel = false;

    }

    public void receberdano()
    {

        intangivel = true;
        vida--; // Reduz a vida
        StartCoroutine(damageplayer());
        if (vida <= 0)
        {
            // Exibe a tela de Game Over
            StartCoroutine(Respawn(0.5f));

        }

    }
    IEnumerator Respawn(float duration)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = Checkpoint;
        spriteRenderer.enabled = true;
        vida = 5;
      
    }
    public void updateChekpoint(Vector2 pos)
    {
        Checkpoint = pos;
    }
}
