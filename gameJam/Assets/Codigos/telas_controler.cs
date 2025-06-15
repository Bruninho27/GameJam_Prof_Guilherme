using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class telas_controler : MonoBehaviour
{
    
        private bool isPaused = false; // Estado atual do jogo (pausado ou não)
        [SerializeField] GameObject Pause; // Referência ao objeto de pausa na cena
       


        // Método para pausar o jogo
        public void pause()
        {
            Pause.SetActive(true); // Ativa o menu de pausa
            Time.timeScale = 0; // Pausa o tempo do jogo
         
            isPaused = true; // Atualiza o estado para pausado

        }

        // Método para voltar ao menu principal
        public void home()
        {
            SceneManager.LoadScene("menu"); // Carrega a cena do menu principal
            Time.timeScale = 1; // Retorna o tempo do jogo para normal
        }

        // Método para retomar o jogo
        public void resume()
        {
            Pause.SetActive(false); // Desativa o menu de pausa
            Time.timeScale = 1; // Retorna o tempo do jogo para normal
           
            isPaused = false; // Atualiza o estado para não pausado

        }



        // Método para iniciar o jogo
        public void jogar()
        {
            StartCoroutine(AguardarECarregar());
        }

        private IEnumerator AguardarECarregar()
        {
            // Espera 2 segundos (ou o tempo que você quiser)
            yield return new WaitForSeconds(.5f);

            // Carrega a cena do jogo
            SceneManager.LoadScene("SceneJogo");
        }

        // Método para sair do jogo
        public void sair()
        {
            StartCoroutine(AguardarEFechar());
        }

        private IEnumerator AguardarEFechar()
        {
            // Espera 2 segundos (ou o tempo que você quiser)
            yield return new WaitForSeconds(.5f);

            // Fecha o aplicativo
            Application.Quit();

            // Para o editor do Unity, descomente a linha abaixo para parar a execução
            // #if UNITY_EDITOR
            //     UnityEditor.EditorApplication.isPlaying = false;
            // #endif
        }
        public void restart()
        {
            SceneManager.LoadScene("SceneJogo");
        }


        // Atualiza a cada frame
        void Update()
        {
            // Verifica se a tecla ESC foi pressionada
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    resume(); // Retoma o jogo se estiver pausado
                }
                else
                {
                    pause(); // Pausa o jogo se estiver em execução
                }
            }
        }

    }
