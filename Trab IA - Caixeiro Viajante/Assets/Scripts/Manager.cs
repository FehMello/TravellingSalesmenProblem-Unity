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

    private int _Populacao = 0; //Numero de individuos(ROTAS) a serem criadas, esse numero é especificado no inspector e não deve ser alterado no código.
    [SerializeField]
    private List <Rota> _Rotas = new List<Rota>(); //O tamanho dessa lista tem que se manter igual à qnt de população durante o programa todo.
    [SerializeField]
    public List<City> _Cidades = new List<City>();//Pra ter controle das cidades
    private City _CityScript;
    [SerializeField]
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

        //Cria primeira população
        for (int i = 0; i < _Populacao; i++)
        {
            _RotaScript = new Rota(i, this);

            _Rotas.Add(_RotaScript);

        }

    }

   
    void Update()
    {
       
        


    }
}
