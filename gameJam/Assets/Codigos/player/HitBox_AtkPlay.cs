using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_AtkPlay : MonoBehaviour
{
    public Player_move player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.IsAttackingOrDashing()) return;

        GameObject obj = collision.gameObject;
        Vector2 direction = (obj.transform.position - player.transform.position).normalized;

        if (obj.CompareTag("Pequeno"))
        {
            player.StartCoroutine(player.HandleSmallObjectHit(obj, direction));
        }
        else if (obj.CompareTag("Grande"))
        {
            Obj_Grande grande = obj.GetComponent<Obj_Grande>();
            if (grande != null)
            {
                grande.LevarHit(direction);
            }
        }
    }
}
