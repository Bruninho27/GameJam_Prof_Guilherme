using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_atk : MonoBehaviour
{
    public player_anim animatk;
    public int damage=10;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaCost = 25f;
   

    [Header("Regeneração")]
    public float staminaRegenRate = 10f;      // quanto regenera por segundo
    public float regenDelay = 2f;             // tempo de espera antes de começar a regenerar
    private float timeSinceLastAttack = 0f;


    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
       
    }

    // Update is called once per frame
    void Update()
    {
        // Atualiza o contador de delay
        timeSinceLastAttack += Time.deltaTime;
        // Só regenera se passou o tempo de delay
        if (timeSinceLastAttack >= regenDelay)
            RegenerateStamina();
      

        if (Input.GetKeyDown(KeyCode.V) && currentStamina >= staminaCost)
        {
            atkPlayer();
        }
    }
    void atkPlayer()
    {
        currentStamina -= staminaCost;
        currentStamina = Mathf.Max(currentStamina, 0f);

        // Reseta o contador de delay
        timeSinceLastAttack = 0f;

        animatk.atk();
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
