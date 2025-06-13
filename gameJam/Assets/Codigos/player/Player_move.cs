using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_move : MonoBehaviour
{

    // Variáveis
    private Rigidbody2D RB; // Referência ao Rigidbody2D para física

    [Header("speed")]
    private float originalSpeed;
    public float tempoLento = 1.5f;
    public float speedLento = 2f;
    public float speed; // Velocidade de movimento do jogador


    [Header("Ação")]
    [SerializeField] private float boostSpeed = 10f; // Velocidade ao acelerar
    private bool isAccelerating = false;
    private bool isAttacking = false;


    [Header("Vida")]
    public bool intangivel; // Indica se o jogador é intangível
    private SpriteRenderer spriteRenderer;
    private bool Hit;
    public GameObject perseguidor; // Referência ao perseguidor
   [SerializeField] private int danoRecebido = 0;

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
    private perseguidorControler perseguidorscrpit;

    public int pontos; // Para pontuação

    private float attackDuration = 0.5f; // tempo que o ataque dura
    private float attackTimer = 0f;


    private void Awake()
    {
        if (perseguidor != null)
        {
            perseguidorscrpit = perseguidor.GetComponent<perseguidorControler>();

            if (perseguidorscrpit == null)
                Debug.LogError("Script perseguidorControler não encontrado no objeto perseguidor.");
        }
        else
        {
            Debug.LogError("Perseguidor não atribuído no Inspector.");
        }

    }

    void Start()
    {
        RB = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
        currentStamina = maxStamina;
        originalSpeed = speed;

      

    }

    // Update is called once per frame
    void Update()
    {


        bool holdingShift = Input.GetKey(KeyCode.LeftShift);// Acelerando?
      
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            // Chama animação de ataque
            anim.atk();
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }

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
        anim.UpdateStates(isAccelerating);

        OnMove(); // Movimento real
        UiManager.Instance.AtualizarStamina(currentStamina, maxStamina);
     


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
        if (intangivel) return;
        intangivel = true;
  
        StartCoroutine(damageplayer());

        danoRecebido++;

        if (danoRecebido == 1)
        {
            // 1º Hit: mensagem de alerta
            UiManager.Instance.MostrarMensagem("Você sentiu algo estranho atrás de você...");
            
        }
        else if (danoRecebido == 2)
        {
            // 2º Hit: ativar perseguidor
            perseguidor.SetActive(true);
            perseguidorscrpit.player = this.transform; // <- ESSENCIAL
            perseguidorscrpit.Ativar();
            UiManager.Instance.MostrarMensagem("Você sentiu algo");
          
        }
        else if (danoRecebido > 2)
        {
            // Hits seguintes: perseguidor avança
           perseguidorscrpit.Avancar();
        }


    }
    public void ReceberColisaoComObstaculo()
    {
        if (!intangivel)
        {
            receberdano(); // usa seu método já existente
            StartCoroutine(ReduzirVelocidadeTemporariamente());
        }
    }



    IEnumerator ReduzirVelocidadeTemporariamente()
    {
        speed = speedLento;
        yield return new WaitForSeconds(tempoLento);
        speed = originalSpeed;
    }
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    public bool EstaAcelerandoOuAtacando()
    {
        return isAccelerating || isAttacking;
    }

    public void AdicionarPontos(int valor)
    {
        pontos += valor;
        UiManager.Instance.AtualizarPontos(pontos);
        Debug.Log("Pontos: " + pontos);
    }

}
