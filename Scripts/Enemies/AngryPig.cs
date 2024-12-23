using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AngryPig : MonoBehaviour
{
    public Transform target;
    public float runSpeed;
    public float walkSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public bool attack;
    public bool levandoDano;
    public int dano;
    public Vector2 direcao;
    public float impulseForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = false;
        levandoDano = false;
        dano = 0;
    }

    private void Update()
    {
        Flip();
        Animations();
        CheckDistance();
        Hurt();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            levandoDano = true;
        }
    }

    private void CheckDistance()
    {
        if (!levandoDano)
        {
            Vector2 targetPosition = target.position;
            Vector2 posicaoAtual = transform.position;
            direcao = targetPosition - posicaoAtual;

            if ((direcao.x <= 5 && direcao.x >= -5) && (direcao.y <= 5 && direcao.y >= -5))
            {
                Vector2 direcaoNova = direcao.normalized;
                rb.velocity = new Vector2(direcaoNova.x * runSpeed, direcaoNova.y);
            }
            else 
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

   
    private void Flip()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Hurt()
    {
        if (levandoDano)
        {
            dano += 1;
            Player.instance.Impulse(impulseForce);
            StartCoroutine(PararInimigo());
        }
    }

    private  IEnumerator PararInimigo()
    {

        yield return new WaitForSeconds(1f);
        levandoDano = false;
    }

    private void Animations()
    {
        if (rb.velocity.x != 0 && !levandoDano)
        {
            animator.Play("AngryPig_run");
        }

        if (rb.velocity.x == 0 && !levandoDano)
        {
            animator.Play("AngryPig_idle");
        }

        if (levandoDano && dano == 1)
        {
            animator.Play("AngryPig_hit1");
        }

        if(levandoDano && dano == 2)
        {
            animator.Play("AngryPig_hit2");
        }
    }
}
