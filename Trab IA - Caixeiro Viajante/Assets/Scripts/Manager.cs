using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //nossa população/individuos q estamos testando n sao as cidades e sim as rotas
    //as rotas (individuos) terao a cidade no array.

    public int qntCidades;//Objetos que os individuos vão 'interagir' para achar a melhor rota
    
    public GameObject CityObj;

    private List <Rota> _Rotas = new List<Rota>(); //O tamanho dessa lista tem que se manter igual à qnt de população durante o programa todo.
    [SerializeField]
    private List<GameObject> _Cidades = new List<GameObject>();//Pra ter controle das cidades
    [SerializeField]
    private int _Populacao = 0;// Numero de individuos a serem criados, esse numero deve se manter fixo.

    void Start()
    {
        //Cria cidades
        for (int i = 0; i < qntCidades; i++)
        {
            GameObject cityTMP;
            Vector3 position = new Vector3(Random.Range(-13.0F, 13.0F), 0, Random.Range(-13.0F, 13.0F));
            cityTMP=Instantiate(CityObj, position, Quaternion.identity) as GameObject;
            _Cidades.Add(cityTMP);
        }

        //int temp = 0;

        //Da ids pras cidades.
        for (int i = 0; i < qntCidades; i++)
        {
            City cidadetemp;
            cidadetemp = _Cidades[i].GetComponent<City>();
            cidadetemp.SetID(i);
            cidadetemp.SetText(cidadetemp.GetID());
            //_Cidades[i].GetComponent<City>().SetID(i);
            //temp=_Cidades[i].GetComponent<City>().GetID();
            //_Cidades[i].GetComponent<City>().SetID(i);

        }

        //Pega numero de cidades 
        _Populacao = qntCidades;
        
        
    }

   
    void Update()
    {
        


    }
}
