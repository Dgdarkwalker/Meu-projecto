using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Movimento Do Jogador")]
    public int velocidade;
    public float movimentoHorizontal;
    public bool indoPraDireita;
    public bool podeMover;
    public bool estaVivo;

    [Header("Referencias")]
    private Rigidbody2D rb;
    private Animator anim;
    public static Player instance;

    [Header("Pulo Do Jogador")]
    public LayerMask layerDoChao;
    public Transform verificadorDeChao;
    public float tamanhoDoRaioDeVerificacao;
    public bool estaNoChao;
    public bool estaPulandoDuplo;
    public int forcaDoPulo;
    public bool animandoPuloDuplo;
    public float fimDaAnimacaoPuloDuplo;
    public float duracaoPuloDuplo;
    public float forcaDeImpulso;

    [Header("WallJump")]
    public LayerMask layerDaParede;
    public Transform verificadorDeParede;
    public bool estaNaParede;
    public bool estaDeslizandoNaParede;
    public bool estaPulandoNaParede;
    public float forcaXDoPuloNaParede;
    public float forcaYDoPuloNaParede;
    public float duracaoDoWallJump;
    public float FimDoPuloNaParede;
    public float velocidadeDeQueda;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        instance = this;
    }

    void Start()
    {
        estaPulandoDuplo = false;
        estaPulandoNaParede = false;
        podeMover = true;
        animandoPuloDuplo = false;
        estaVivo = true;
        estaDeslizandoNaParede = false;
    }

    void Update()
    {
        Move();
        Flip();
        Animations();
        Jump();
        Walljump();
    }

    private void Move()
    {
        if (podeMover && estaVivo && !estaDeslizandoNaParede)
        {
            movimentoHorizontal = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(movimentoHorizontal * velocidade, rb.velocity.y);
        }

        if (estaNoChao || !estaNaParede)
        {
            estaDeslizandoNaParede = false;
        }
    }

    private void Flip()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
            indoPraDireita = false;
        }

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1f,1f,1f);
            indoPraDireita = true;
        }
    }

    private void Jump()
    {
        estaNoChao = Physics2D.OverlapCircle(verificadorDeChao.position, tamanhoDoRaioDeVerificacao, layerDoChao);

        // Pulo
        if (estaNoChao )
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(0f, forcaDoPulo);
            }
        }

        // Pulo Duplo
        else if (Input.GetButtonDown("Jump") && !estaPulandoDuplo && !estaNoChao && !estaNaParede)
        {
            rb.velocity =  new Vector2 (rb.velocity.x, forcaDoPulo);
            estaPulandoDuplo = true;
            fimDaAnimacaoPuloDuplo = Time.time + duracaoPuloDuplo;
            animandoPuloDuplo = true;
        }

        if(estaNoChao || estaNaParede)
        {
            estaPulandoDuplo = false;
        }

    }

    private void Walljump()
    {
        estaNaParede = Physics2D.OverlapCircle(verificadorDeParede.position, tamanhoDoRaioDeVerificacao, layerDaParede);

        if (Input.GetButtonDown("Jump") && estaNaParede && !estaNoChao)
        {
            
            podeMover = false;
            FimDoPuloNaParede = Time.time + duracaoDoWallJump;
            estaPulandoNaParede = true;

            if (estaPulandoNaParede)
            {
                if (indoPraDireita)
                {
                    rb.velocity = Vector2.zero;
                    rb.velocity = new Vector2(-forcaXDoPuloNaParede, forcaYDoPuloNaParede);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    rb.velocity = new Vector2(forcaXDoPuloNaParede, forcaYDoPuloNaParede);
                }
            }
        }

        if(estaNaParede && !estaNoChao && !estaPulandoNaParede)
        {
            rb.velocity = new Vector2(rb.velocity.x, velocidadeDeQueda);
            estaDeslizandoNaParede = true;
        }

        if (estaPulandoNaParede)
        {
            if(Time.time > FimDoPuloNaParede)
            {
                estaPulandoNaParede = false;
            }
        }

        if(!estaPulandoNaParede && !podeMover)
        {
            if(Input.GetAxis("Horizontal") != 0 || estaNoChao)
            {
                podeMover = true;
            }
        }

    }

    public void HurtPlayer()
    {
        estaVivo = false;
        GameManager.instance.GameOver();
    }

    public void Impulse()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(rb.velocity.x, forcaDeImpulso), ForceMode2D.Impulse);
    }



    private void Animations()
    {
        // Correndo
        if(movimentoHorizontal != 0 && estaNoChao && estaVivo)
        {
            anim.Play("Player_run");
        }

        // Parado
        if (movimentoHorizontal == 0 && estaNoChao && estaVivo)
        {
            anim.Play("Player_idle");
        }

        // Pulando
        if (!estaNoChao && rb.velocity.y > 0 && !animandoPuloDuplo && !estaNaParede && estaVivo && !estaDeslizandoNaParede)
        {
            anim.Play("Player_jump");
        }

        // Caindo
        if(!estaNoChao && rb.velocity.y < 0 && !animandoPuloDuplo && !estaNaParede && estaVivo)
        {
            anim.Play("Player_fall");
        }

        // Pulo Duplo
        if(!estaNoChao && estaPulandoDuplo && !estaNaParede && animandoPuloDuplo && estaVivo)
        {
            anim.Play("Player_doublejump");
            
            if(Time.time > fimDaAnimacaoPuloDuplo)
            {
                animandoPuloDuplo = false;
            }
        }

        // Deslizando na Parede
        if(!estaNoChao && estaNaParede && estaVivo && estaDeslizandoNaParede)
        {
            anim.Play("Player_walljump");
        }

        // Morrendo
        if (!estaVivo)
        {
            anim.Play("Player_hit");
        }
    }
}
