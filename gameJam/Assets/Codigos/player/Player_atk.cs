using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player_atk : MonoBehaviour
{
    public Animator animatk;
    public GameObject gameover; // Referência ao objeto de Game Over

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaCost = 25f;
   

    [Header("Regeneração")]
    public float staminaRegenRate = 10f;      // quanto regenera por segundo
    public float regenDelay = 2f;             // tempo de espera antes de começar a regenerar
    private float timeSinceLastAttack = 0f;

    [Header("vida")]
    public bool intangivel; // Indica se o jogador é intangível
    private SpriteRenderer spriteRenderer;
    public int vidamax = 100;
    public int vidaAtual;

    [Header("Movimentação")]
    public float speed = 5f;

    [Header("Limites Horizontais (por objetos)")]
    public Transform limiteEsquerdo;
    public Transform limiteDireito;

  //  private CinemachineImpulseSource source;



    // Start is called before the first frame update
    void Start()
    {
        vidaAtual = vidamax;
        currentStamina = maxStamina;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
      

    }

    // Update is called once per frame
    void Update()
    {

        float movimento = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // Move no eixo X
        transform.position += new Vector3(movimento, 0f, 0f);

        // Limita usando a posição dos objetos
        float limiteMinX = limiteEsquerdo.position.x;
        float limiteMaxX = limiteDireito.position.x;

        // Limita a posição no eixo X
        float posXLimitado = Mathf.Clamp(transform.position.x, limiteMinX, limiteMaxX);
        transform.position = new Vector3(posXLimitado, transform.position.y, transform.position.z);


        // Atualiza o contador de delay
        timeSinceLastAttack += Time.deltaTime;
        // Só regenera se passou o tempo de delay
        if (timeSinceLastAttack >= regenDelay)
            RegenerateStamina();
      

        if (Input.GetKeyDown(KeyCode.Space) && currentStamina >= staminaCost)
        {
            atkPlayer();
        }

       
        UiManager.Instance.AtualizarStamina(currentStamina, maxStamina);
    }
    void atkPlayer()
    {
        currentStamina -= staminaCost;
        currentStamina = Mathf.Max(currentStamina, 0f);
        animatk.SetTrigger("atak");

        // Reseta o contador de delay
        timeSinceLastAttack = 0f;
        



    }
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
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
    public void receberdano(int dano)
    {
        if (intangivel) return;
        vidaAtual -= dano;
        UiManager.Instance.AtualizavidaPalyer(vidaAtual, vidamax);
        intangivel = true;
        StartCoroutine(damageplayer());

        
        if (vidaAtual <= 0)
        {
            //chama tela de morte
            showgameover();
        }
      


    }
    public void showgameover()
    {

        gameover.SetActive(true); // Ativa o objeto de Game Over
        SceneManager.LoadScene("boss");


    }
}
