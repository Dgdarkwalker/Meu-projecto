using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.Impulse();
            anim.Play("Trampoline_jump");
            StartCoroutine(Temporizador());
        }
    }

    IEnumerator Temporizador()
    {
        yield return new WaitForSeconds(0.5f);
        anim.Play("Trampoline_idle");
    }
}
