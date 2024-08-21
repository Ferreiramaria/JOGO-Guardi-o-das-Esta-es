using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimentacaobasica : MonoBehaviour
{
   

    [Header("Conf Player")]
    public float velocidade;
    public float movimentoHorizontal;
    private Rigidbody2D rbPlayer;
    public float forcaPulo;
    public Transform posicaoSensor;
    public bool Sensor;
    private Animator anim;
    public bool verificarDirecaoPersonagem;

    [Header("Conf Tiro")]
    public GameObject municao;
    public Transform posicaoTiro;
    public float velocityTiro;
    public LayerMask chao;

    [Header("Conf Vida")]
    public int contadordevida;
    public int vidaAtual;
    public TextMeshProUGUI textVida;

    [Header("Conf Munição")]
    public int municaoAtual = 5;
    public TextMeshProUGUI textMunicao;


    public int moeda;
    public TextMeshProUGUI textmoeda;
    //GameOver
    public bool isGameOver;
    public GameObject gameOverScreen;
    public GameObject fade;

    public bool pausa;

    public GameObject fadeIn;

    private void Awake()
    {
        //fade.SetActive(true);
        Time.timeScale = 1;
    }


    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        vidaAtual = 10;
        textVida.text = vidaAtual.ToString();

        municaoAtual = 10;
        textMunicao.text = municaoAtual.ToString();

        //_Salvar = FindObjectOfType(typeof(Salvar)) as Salvar;

    }

    // Update is called once per frame
    void Update()
    {
        verificarChao();
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        rbPlayer.velocity = new Vector2(movimentoHorizontal*velocidade, rbPlayer.velocity.y);

        //mudar direçâo do personagem

        //if(movimentoHorizontal >0 && rbPlayer.velocity.y > 0)

        if (movimentoHorizontal > 0 && verificarDirecaoPersonagem == true && pausa == false)
        {
            flip();

        }
        else if (movimentoHorizontal < 0 && verificarDirecaoPersonagem == false && pausa == false)
        {
            flip();
        }

        if (Input.GetButtonDown("Jump") && Sensor==true)
        {
           rbPlayer.AddForce(new Vector2(0, forcaPulo));
        }
        if (Input.GetMouseButtonDown(0))
        {
            Atirar();
        }

        anim.SetInteger("Run", (int)movimentoHorizontal);
        anim.SetBool("Sensor", Sensor);

        if(isGameOver == true)
        {
            gameOverScreen.SetActive(true);
            velocidade = 0;
            velocityTiro = 0;
            pausa = true;
           // Time.timeScale = 0;
        }
        else
        {
            gameOverScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void verificarChao()
    {
        Sensor = Physics2D.OverlapCircle(posicaoSensor.position, 0.2f,chao);
    }
    public void flip()
    {
        verificarDirecaoPersonagem = !verificarDirecaoPersonagem;

        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        velocityTiro *= -1;

        municao.GetComponent<SpriteRenderer>().flipX = verificarDirecaoPersonagem;
    }
    public void Atirar()
    {
        if(municaoAtual !=0)
        {
            municaoAtual--;
            textMunicao.text = municaoAtual.ToString();
            GameObject temporario = Instantiate(municao);
            temporario.transform.position = posicaoTiro.position;
            temporario.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityTiro, 0);
        }
        else
        {
            municaoAtual = 0;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "chao")
        {
            isGameOver = true;
        }

        if (collision.gameObject.tag == "lava")
        {
            anim.SetBool("morreu", true);
            anim.SetTrigger("morte");
            Debug.Log("Morreu");
            isGameOver = true;
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ganhar vida
        if (collision.gameObject.tag == "vida")
        {
            isGameOver = true;
            contadordevida++;
            vidaAtual = vidaAtual + contadordevida;
            textVida.text = vidaAtual.ToString();
            Destroy(collision.gameObject);
        }
        //Perder vida
        if (collision.gameObject.tag == "inimigo")
        {
  
            vidaAtual--;
            textVida.text = vidaAtual.ToString();
            Destroy(collision.gameObject);

        }
        if(collision.gameObject.tag == "Recarregar")
        {

            municaoAtual = municaoAtual + 5;
            textMunicao.text = municaoAtual.ToString();
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.tag =="mudarfase")
        {

           SceneManager.LoadScene("fase 1");

        }

        if (collision.gameObject.tag == "coin")
        {

            moeda++;
            textmoeda.text = moeda.ToString();
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.tag == "mudarfase")
        {
            
            fadeIn.SetActive(true);

        }
    }

    public void Restart()
    {
 
        isGameOver = false;

        pausa = false;
        
        SceneManager.LoadScene("fase1"); 

    }


   



}
