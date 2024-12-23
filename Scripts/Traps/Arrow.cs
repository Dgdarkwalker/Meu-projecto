using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Animator anim;
    public float impulseForce;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.Impulse(impulseForce);
            anim.Play("Arrow_hit");
            StartCoroutine(DestroyArrow());
        }
        
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(1f/6);
        Destroy(this.gameObject);
    }
}
