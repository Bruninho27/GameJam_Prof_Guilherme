using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Pequeno : Obj_destrutivel
{
    // Start is called before the first frame update
    public float forca = 800f;

    public override void LevarHit(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(direction * forca);

        TocarAnimacao("Destruir");
        DestruirComDelay(0.3f);
    }
}
