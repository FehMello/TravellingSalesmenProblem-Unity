using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //nossa população/individuos q estamos testando n sao as cidades e sim as rotas
    //as rotas (individuos) terao a cidade no array.
    public Material OriginMaterial;
    public int qntCidades;//Objetos que os individuos vão 'interagir' para achar a melhor rota.   
    public GameObject CityObj; //Prefab da cidade
    public int Populacao = 0; //Numero de individuos(ROTAS) a serem criadas, esse numero é especificado no inspector e não deve ser alterado no código.
    public int Geracao = 0;
    public int firstBest = 0;
    public int secondBest = 0;

    [SerializeField]
    private List <Rota> _Rotas = new List<Rota>(); //O tamanho dessa lista tem que se manter igual à qnt de população durante o programa todo.
    [SerializeField]
    public List<City> _Cidades = new List<City>();//Pra ter controle das cidades
    private City _CityScript;
    private float MenorDist; //Variavel modifica dentro de mais de um metodo

    int SelectFirstFit()
    {
        //Variaveis que guardam index
        bool Equal = false;
        int Best = 0;


        //Prioriza achar algum que começe e termine na origem
        for (int i = 0; i < _Rotas.Count; i++)
        {
            if (_Rotas[i].dna[0].GetOrigin() == true)
            {
                //Compara se a cidade do primeiro dna é igual a do ultimo
                if (_Rotas[i].dna[0].GetID() == _Rotas[i].dna[_Rotas[i].dna.Count-1].GetID()) 
                {
                    _Rotas[i].SetHasOrigin(true);
                    Best = i;
                    Equal = true;

                }

            }         
        }
 

        if (Equal == false) //Se tiver um igual, tenta achar o que tiver menor distancia 
        {
            MenorDist = float.MaxValue; //Var auxiliar

            for (int i = 0; i < _Rotas.Count; i++)
            {
                if (_Rotas[i].CalculoDistRota() < MenorDist && _Rotas[i].dna[0].GetOrigin() == true || _Rotas[i].dna[_Rotas[i].dna.Count - 1].GetOrigin() == true)
                {

                    MenorDist = _Rotas[i].CalculoDistRota();
                    Best = i;

                }


            }

        }


       
        return Best;

    }

    int SelectSecondFit(int First)
    {
  
        int FirstBest = First;
        int SecondBest=0;

        bool Equal = false;

        //Prioriza tentar achar quem tem dna de inicio e fim iguais
        for (int i = 0; i < _Rotas.Count; i++)
        {
            if (_Rotas[i].dna[0].GetOrigin() == true && i!=FirstBest)
            {
                //Compara se a cidade do primeiro dna é igual a do ultimo
                if (_Rotas[i].dna[0].GetID() == _Rotas[i].dna[_Rotas[i].dna.Count - 1].GetID())
                {
                    _Rotas[i].SetHasOrigin(true);
                    SecondBest = i;
                    Equal = true;

                }

            }
        }


        if (Equal == false) //Se n tiver um igual, tenta considerar um que tem pelo menos 1 igual
        {
            MenorDist = float.MaxValue; //Var auxiliar

            for (int i = 0; i < _Rotas.Count; i++)
            {
                if (i != FirstBest)
                {
                    if (_Rotas[i].CalculoDistRota() < MenorDist && _Rotas[i].dna[0].GetOrigin() == true || _Rotas[i].dna[_Rotas[i].dna.Count - 1].GetOrigin() == true)
                    {

                        MenorDist = _Rotas[i].CalculoDistRota();
                        SecondBest = i;

                    }

                }

            }

        }
 

        return SecondBest;

    }

    void Crossover(int first, int second)
    {
        Rota Child = new Rota(this,false);
        Child.dna.Clear();

        //Troca os valores dos genes
        for (int i = 0; i < qntCidades+1; i++)
        {
            int aux = Random.Range(0, 1); //Condicao random
            if (aux == 0)
            {
                Child.dna.Add(_Rotas[first].dna[i]);
            }
            else
            {
                Child.dna.Add(_Rotas[second].dna[i + 1]);
            }
          
        }

        _Rotas.Add(Child);
    }

    void RetiraMenosFit()
    {
        //Menos fit eh quem tem maior distancia percorrida e nao tem cidade de origem nem no inicio nem fim
        int menosfit = 0;

        for (int i = 0; i < _Rotas.Count; i++)
        {
            for (int j = 1; j < _Rotas.Count; j++)
            {

                if (_Rotas[i].CalculoDistRota() > _Rotas[j].CalculoDistRota() && _Rotas[i].GetHasOrigin()!=true)
                {
                    menosfit = i;
                }

            }

        }

        _Rotas.RemoveAt(menosfit);

    }


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

        int aux = Random.Range(0, qntCidades);
        _Cidades[aux].SetOrigin(true); //Escolhe cidade aleatória para ser origem
        _Cidades[aux].GetComponent<MeshRenderer>().material = OriginMaterial;

        //Cria primeira população
        for (int i = 0; i < Populacao; i++)
        {            
            Rota Rota = new Rota(this,false);
            _Rotas.Add(Rota);
        }

        //TESTE distancias
        for (int i = 0; i < _Rotas.Count; i++)
        {
            _Rotas[i].MostrarCromo();
            Debug.Log("Rota " + i + " tem distancia " + _Rotas[i].CalculoDistRota());
        }



    }


    void Update()
    {
        if (Geracao < 100)
        {
            firstBest = SelectFirstFit();

            secondBest = SelectSecondFit(firstBest);

            Crossover(firstBest, secondBest);

            RetiraMenosFit();

            _Rotas[firstBest].DrawRoute();

            Geracao++;
        }

        Debug.Log("O mais fit eh ");
        _Rotas[firstBest].MostrarCromo();

    }


}
