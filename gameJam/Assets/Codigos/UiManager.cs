using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [Header("Mensagem")]
    public Text mensagemTexto;
    public float tempoMensagem = 2f;

    [Header("HUds")]
    public Image staminaImage;  // UI do tipo "Image" com Fill
    public Image VidaPlayerImage;  // UI do tipo "Image" com Fill
    public Image VidaBossImage;  // UI do tipo "Image" com Fill


    [Header("Pontuação")]
    public TextMeshProUGUI pontosText; // UI para mostrar pontuação

    void Awake()
    {
        Instance = this;
        if (mensagemTexto != null)
            mensagemTexto.text = "";
    }
    
    
    public void MostrarMensagem(string texto)
    {
        StopAllCoroutines();
        StartCoroutine(ExibirMensagem(texto));
    }

    IEnumerator ExibirMensagem(string texto)
    {
        mensagemTexto.text = texto;
        yield return new WaitForSeconds(tempoMensagem);
        mensagemTexto.text = "";
    }
    public void AtualizarStamina(float atual, float max)
    {
        if (staminaImage != null)
            staminaImage.fillAmount = atual / max;
    }

    public void AtualizavidaBoss(float atual, float max)
    {
        if(VidaBossImage!=null)
            VidaBossImage.fillAmount = atual / max;
    }
    public void AtualizavidaPalyer(float atual , float max)
    {
        if(VidaPlayerImage!=null)
            VidaPlayerImage.fillAmount = atual / max;
    }

    public void AtualizarPontos(int pontos)
    {
        if (pontosText != null)
            pontosText.text = "Pontos: " + pontos;
    }
}
