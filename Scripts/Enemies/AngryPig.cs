using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AngryPig : MonoBehaviour
{
    public Transform target;
    public float runSpeed;
    public float walkSpeed;
    private Rigidbody2D rigidbody;
    private Animator animator;
    public bool attack;
    public bool levandoDano;
    public int dano;
    public Vector2 direcao;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

        if (attack && !levandoDano)
        {
            Attack();
        }

        if (levandoDano)
        {
            Hurt();
        }
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
        Vector2 targetPosition = target.position;
        Vector2 posicaoAtual = transform.position;
        direcao = targetPosition - posicaoAtual;
        
        if ((direcao.x <= 5 && direcao.x >= -5) && (direcao.y <= 5 && direcao.y >= -5))
        {
            attack = true;

        }

        else
        {
            attack=false;
            rigidbody.velocity = Vector2.zero;
        }
    }

    private void Attack()
    {
        Vector2 direcaoNova = direcao.normalized;
        rigidbody.velocity = new Vector2(direcaoNova.x * runSpeed, direcaoNova.y);
    }

    private void Walk()
    {

    }

    private void Flip()
    {
        if (rigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Hurt()
    {
        if (dano < 1)
        {
            rigidbody.velocity = Vector2.zero;
            animator.Play("AngryPig_hit1");
            dano += 1;
        }

        if (dano == 1)
        {
            rigidbody.velocity = Vector2.zero;
            animator.Play("AngryPig_hit2");
            dano +=1;
        }
        
        if (dano == 2)
        {
            Destroy(this.gameObject);
        }
    }

    private void Animations()
    {
        if (rigidbody.velocity.x != 0 && !levandoDano)
        {
            animator.Play("AngryPig_run");
        }

        if (rigidbody.velocity.x == 0 && !levandoDano)
        {
            animator.Play("AngryPig_idle");
        }
    }
}
