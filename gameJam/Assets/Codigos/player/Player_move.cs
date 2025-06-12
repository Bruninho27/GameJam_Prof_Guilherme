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
        // Avisar o script de animação
        anim.UpdateStates(isAccelerating, isAttacking);

        OnMove(); // Movimento real


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        Vector2 direction = (obj.transform.position - transform.position).normalized;

        if (obj.CompareTag("Pequeno"))
        {
            if (isAccelerating || isAttacking)
                StartCoroutine(HandleSmallObjectHit(obj, direction));
            else
                ApplyKnockback(direction);
        }
        else if (obj.CompareTag("Grande"))
        {
            if (isAccelerating || isAttacking)
            {
                var bigObj = obj.GetComponent<Obj_Grande>();
                if (bigObj != null)
                    bigObj.LevarHit(direction);
            }
            else
                ApplyKnockback(direction);
        }
    }
   public  IEnumerator HandleSmallObjectHit(GameObject obj, Vector2 direction)
    {
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(direction * 800f);

        var animObj = obj.GetComponent<Animator>();
        if (animObj)
            animObj.SetTrigger("Destruir");

        yield return new WaitForSeconds(0.3f);
        Destroy(obj);
    }

    void ApplyKnockback(Vector2 direction)
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(-direction * 300f);
    }
    void OnMove()
    {
        float currentSpeed = isAccelerating ? boostSpeed : speed;
        RB.velocity = new Vector2(currentSpeed, RB.velocity.y);
        // Atualiza a velocidade horizontal do jogador
       
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
    public bool IsAttackingOrDashing()
    {
        return isAccelerating || isAttacking;
    }


}
