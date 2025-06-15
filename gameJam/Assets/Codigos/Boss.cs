using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Boss : MonoBehaviour
{
    public int vidaMaxima = 500;
    public int vidaAtual;

    private SpriteRenderer spriteRenderer;
    public GameObject telafinal;
    private CinemachineImpulseSource inpulseSorce;

    void Start()
    {
        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        inpulseSorce = GetComponent<CinemachineImpulseSource>();
    }

    public void LevarDano(int dano)
    {
        camerameche.instace.camerashake(inpulseSorce);
        vidaAtual -= dano;
        if (vidaAtual < 0) vidaAtual = 0;

        // Atualiza a UI
        UiManager.Instance.AtualizavidaBoss(vidaAtual, vidaMaxima);

        // Feedback visual
        StartCoroutine(Piscar());

        // Aqui você pode colocar morte, caso a vida zere
        if (vidaAtual == 0)
        {
            // Morreu
            showTelafinal();
        }
    }

    private System.Collections.IEnumerator Piscar()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = original;
    }
    public void showTelafinal()
    {
        telafinal.SetActive(true);
      
        SceneManager.LoadScene("menu");
    }
}
