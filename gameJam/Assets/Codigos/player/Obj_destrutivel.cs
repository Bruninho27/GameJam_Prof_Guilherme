using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_destrutivel : MonoBehaviour
{
    public Animator animator;

    public virtual void LevarHit(Vector2 direction) { }

    public void TocarAnimacao(string nomeTrigger)
    {
        if (animator != null)
            animator.SetTrigger(nomeTrigger);
    }

    public void DestruirComDelay(float delay)
    {
        StartCoroutine(DestroyAfterDelay(delay));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
