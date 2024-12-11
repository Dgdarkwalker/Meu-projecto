using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private int velocidadeDeMovimento;
    [SerializeField] private float movimentoHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Animations();
    }

    private void Move()
    {
        movimentoHorizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(movimentoHorizontal * velocidadeDeMovimento, rb.velocity.y);
    }

    private void Animations()
    {
        if(movimentoHorizontal != 0)
        {
            animator.Play("teste");
        }

        if(movimentoHorizontal == 0)
        {
            animator.Play("idle");
        }
    }
}
