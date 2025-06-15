using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossShoter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform pontoDisparo; // Um empty game object no boss que indica o ponto de disparo

    [Header("�rea de queda dos proj�teis")]
    public Vector2 areaMin; // canto inferior esquerdo da �rea onde os proj�teis podem cair
    public Vector2 areaMax; // canto superior direito da �rea

    public float intervaloDisparo = 2f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= intervaloDisparo)
        {
            DispararProjetil();
            timer = 0;
        }
    }
    void DispararProjetil()
    {
        // Instancia o proj�til na posi��o do ponto de disparo
        GameObject proj = Instantiate(projectilePrefab, pontoDisparo.position, Quaternion.identity);

        // Define uma posi��o alvo aleat�ria dentro da �rea
        Vector2 posAlvo = new Vector2(
            Random.Range(areaMin.x, areaMax.x),
            Random.Range(areaMin.y, areaMax.y)
        );

        // Calcula a dire��o do proj�til, do ponto de disparo para o alvo
        Vector3 direcao = (posAlvo - (Vector2)pontoDisparo.position).normalized;

        // Configura a dire��o do proj�til
        projetil projScript = proj.GetComponent<projetil>();
        if (projScript != null)
        {
            projScript.SetDirection(direcao);
        }
    }
}
