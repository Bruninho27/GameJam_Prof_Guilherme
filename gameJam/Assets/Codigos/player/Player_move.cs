using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{

    // Variáveis
    [Header("Variáveis para mover")]
    private Rigidbody2D RB; // Referência ao Rigidbody2D para física
    public float speed; // Velocidade de movimento do jogador
  

    [Header("Ação")]
    [SerializeField] private float boostSpeed = 10f; // Velocidade ao acelerar
    private bool isAccelerating = false;
    private bool isAttacking = false;


    [Header("Vida")]
    public bool intangivel; // Indica se o jogador é intangível
    private SpriteRenderer spriteRenderer;
    private bool Hit;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaCost = 25f;


    [Header("Regeneração")]
    public float staminaRegenRate = 10f;      // quanto regenera por segundo
    public float regenDelay = 2f;             // tempo de espera antes de começar a regenerar
    private float timeSinceLastAttack = 0f;

    public player_anim anim;

    private bool podeEmpurrar = false;
    private float tempoEmpurrao = 0.5f; // meio segundo de empurro
    private float empurraoTimer = 0f;

    public float forca = 50f;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
        currentStamina = maxStamina;

    }

    // Update is called once per frame
    void Update()
    {


        bool holdingShift = Input.GetKey(KeyCode.LeftShift);// Acelerando?
        isAttacking = Input.GetKeyDown(KeyCode.Space); // Atacando?

        // Gasta stamina se estiver acelerando
        if (holdingShift && currentStamina >= staminaCost * Time.deltaTime)
        {
            isAccelerating = true;
            currentStamina -= staminaCost * Time.deltaTime;
            timeSinceLastAttack = 0f; // zera o tempo de espera pra regenerar
        }
        else
        {
            isAccelerating = false;
            timeSinceLastAttack += Time.deltaTime;

            // Regenera após um tempo parado
            if (timeSinceLastAttack >= regenDelay)
            {
                RegenerateStamina();
            }
        }
        if (isAccelerating || Input.GetKeyDown(KeyCode.Space))
        {
            // Atacando enquanto acelera
            podeEmpurrar = true;
            empurraoTimer = tempoEmpurrao;
        }
        if (podeEmpurrar)
        {
            empurraoTimer -= Time.deltaTime;
            if (empurraoTimer <= 0f)
                podeEmpurrar = false;
        }
        // Avisar o script de animação
        anim.UpdateStates(isAccelerating, isAttacking);

        OnMove(); // Movimento real


    }
   
    void OnMove()
    {
        float currentSpeed = isAccelerating ? boostSpeed : speed;
        RB.velocity = new Vector2(currentSpeed, RB.velocity.y);
        // Atualiza a velocidade horizontal do jogador
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (podeEmpurrar && other.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody2D obstacleRb = other.GetComponent<Rigidbody2D>();
            if (obstacleRb != null)
            {
                bool paraEsquerda = (other.transform.position.x < transform.position.x); // ex: se obstáculo tá à esquerda do player

                Vector2 bias = paraEsquerda ? new Vector2(-0.5f, 1f) : new Vector2(0.5f, 1f);

                Debug.Log("Empurrando: " + other.name);
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                Vector2 finalDirection = (pushDirection + bias).normalized;

                obstacleRb.AddForce(finalDirection * forca, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("Objeto colidido não tem Rigidbody2D: " + other.name);
            }
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
  
        StartCoroutine(damageplayer());
   

    }
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }
   


}
