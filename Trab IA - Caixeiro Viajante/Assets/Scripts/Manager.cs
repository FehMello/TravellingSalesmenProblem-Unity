using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //nossa população/individuos q estamos testando n sao as cidades e sim as rotas
    //as rotas (individuos) terao a cidade no array.

    public int qntCidades;//Objetos que os individuos vão 'interagir' para achar a melhor rota.   
    public GameObject CityObj; //Prefab da cidade
    public int Populacao = 0; //Numero de individuos(ROTAS) a serem criadas, esse numero é especificado no inspector e não deve ser alterado no código.

    [SerializeField]
    private List <Rota> _Rotas = new List<Rota>(); //O tamanho dessa lista tem que se manter igual à qnt de população durante o programa todo.
    [SerializeField]
    public List<City> _Cidades = new List<City>();//Pra ter controle das cidades
    private City _CityScript;
    private Rota _RotaScript;

    

    void Start()
    {
  
        //Cria cidades e adiciona na lista
        for (int i = 0; i < qntCidades; i++)
        {
            GameObject CityTMP;
            Vector3 position = new Vector3(Random.Range(-13.0F, 13.0F), 0, Random.Range(-13.0F, 13.0F)); //Random position
            CityTMP=Instantiate(CityObj, position, Quaternion.identity) as GameObject; //Cria um novo gameobject baseado no prefab CityObj.
            _CityScript = CityTMP.GetComponent<City>(); //Para não precisar repetir getcomponent durante o restante do script, pois é pesado
            _Cidades.Add(_CityScript);
        }

        //Da ids pras cidades
        for (int i = 0; i < qntCidades; i++)
        {
            _Cidades[i].SetID(i);
            _Cidades[i].SetText(_Cidades[i].GetID());
         
        }

        _Cidades[Random.Range(0, qntCidades)].SetOrigin(true); //Escolhe cidade aleatória para ser origem

        //Cria primeira população
        for (int i = 0; i < Populacao; i++)
        {
            
            _RotaScript = new Rota(this);
            _Rotas.Add(_RotaScript);

        }

        //_Rotas[0].MostrarRota();
        //Debug.Log("This route dist is " + _Rotas[0].CalculoDistRota());

        for (int i = 0; i < _Rotas.Count; i++)
        {
            _Rotas[i].MostrarRota();
            Debug.Log("Rota " + i + " tem distancia " + _Rotas[i].CalculoDistRota());
        }

        //TESTE PRA VER SE ACHA BEST FIT BASEADO NA DISTANCIA DESSA POP INICIAL
        //tem q da uma arrumada nas comparacao pq ele ta botando o msm fit como best e second best
        int BestFitIndex = 0;
        int SecondBestFitIndex = 0;

        for (int i = 0; i < _Rotas.Count; i++)
        {
            for (int j = 0; j < _Rotas.Count; j++)
            {
                if (_Rotas[i].CalculoDistRota() > _Rotas[j].CalculoDistRota())
                {
                    BestFitIndex = i;
                    SecondBestFitIndex = j;
                }
                else
                {
                    BestFitIndex = j;
                    SecondBestFitIndex = i;
                }
            }
        }

        Debug.Log("Rota " + BestFitIndex + " tem melhor fit.");
        Debug.Log("Rota " + SecondBestFitIndex + " tem SEGUNDO melhor fit.");

    }

   
    void Update()
    {
        //sequencia de comandos por geração
        //1) olhar rotas e ver melhor fit
        //2) pegar 2 melhores e mistura
        //3) sofrimento

        //mostra as rotas
       



        




    }


}
