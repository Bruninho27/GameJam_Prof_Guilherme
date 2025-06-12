using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Grande :Obj_destrutivel
{
    private int hitCount = 0;

    public float forca = 800f;
    public float forcaleve = 300f;
    public override void LevarHit(Vector2 direction)
    {
        hitCount++;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (hitCount == 1)
        {
            rb?.AddForce(direction * forcaleve);
          TocarAnimacao("Hit1");
        }
        else if (hitCount >= 2)
        {
            rb?.AddForce(direction * forca);
            TocarAnimacao("Destruir");
            DestruirComDelay(0.4f);
        }
    }
}
