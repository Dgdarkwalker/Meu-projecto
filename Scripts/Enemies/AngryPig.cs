using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AngryPig : MonoBehaviour
{
    public Transform target;
    public float runSpeed;
    public bool running;
    public float walkSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public bool levandoDano;
    public Vector2 direcao;
    public float impulseForce;
    public bool indoPraDireita;
    public bool estaNaParede;
    public Transform verificadorDeParedes;
    public float tamanhoDoRaio;
    public LayerMask layerDaParede;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        levandoDano = false;
    }

    private void Update()
    {
        Flip();
        Animations();
        Walk();

        if (!levandoDano)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            levandoDano = true;
            Hurt();
        }
    }

    private void Attack()
    {
        Vector2 targetposition = target.position;
        Vector2 posicaoatual = transform.position;
        direcao = targetposition - posicaoatual;

        if ((direcao.x <= 5 && direcao.x >= -5) && (direcao.y <= 5 && direcao.y >= -5))
        {
            running = true;
            Vector2 direcaonova = direcao.normalized;
            rb.velocity = new Vector2(direcaonova.x * runSpeed, direcaonova.y);
        }
        else
        {
            running = false;
            Walk();
        }
    }

    private void Walk()
    {
        estaNaParede = Physics2D.OverlapCircle(verificadorDeParedes.position, tamanhoDoRaio, layerDaParede);

        if (!levandoDano)
        {
            if (estaNaParede)
            {
                if (indoPraDireita)
                {
                    rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
                }
            }

            if (!estaNaParede)
            {
                if (indoPraDireita)
                {
                    rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
                }
            }
        }
    }

   
    private void Flip()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            indoPraDireita = true;

        }
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            indoPraDireita = false;
        }
    }

    private void Hurt()
    {
        Player.instance.Impulse(impulseForce);
        StartCoroutine(PararInimigo());
    }

    private  IEnumerator PararInimigo()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        levandoDano = false;
    }

    private void Animations()
    {
        if (rb.velocity.x == 0 && !levandoDano && !running)
        {
            animator.Play("AngryPig_idle");
        }

        if (!levandoDano && rb.velocity.x != 0 && running)
        {
            animator.Play("AngryPig_run");
        }

        if (levandoDano)
        {
            animator.Play("AngryPig_hit1");
        }

        if (!running)
        {
            animator.Play("AngryPig_walk");
        }

    }
}
