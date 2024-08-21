using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentoPlataforma1 : MonoBehaviour
{
    public Transform posicaoDireita, posicaoEsquerda;
    public float velocidade;
    public float direcao;
    public float distancia1, distancia2;
    void Start()
    {
        direcao = 1;
    }

    // Update is called once per frame
    void Update()
    {
        distancia1 = Vector3.Distance(transform.position, posicaoDireita.position);
        distancia2 = Vector3.Distance(transform.position,posicaoEsquerda.position);

        if(distancia1 < 1)
        {

            direcao = -1;
        }else if(distancia2 < 1)
        {
            direcao = 1;
        }

        transform.localScale =new Vector3(-direcao, transform.localScale.y, transform.localScale.z);

        transform.Translate(new Vector3(1,0,0) * velocidade * direcao * Time.deltaTime);
    }
}
