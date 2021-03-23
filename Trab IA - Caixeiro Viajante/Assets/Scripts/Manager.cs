using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //CONVENCIONAR CIDADE 0 COMO ORIGEM SEMPRE, USAR QNT CIDADES COMO NUM GENES
    //MUDAR O DRAW - PEGAR
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
        //Variavel guarda index
        int Best = 0;
     
        MenorDist = float.MaxValue; //Var auxiliar

        for (int i = 0; i < _Rotas.Count; i++)
        {
            if (_Rotas[i].CalculoDistRota() < MenorDist)
            {

                MenorDist = _Rotas[i].CalculoDistRota();
                Best = i;

            }

        }

        Debug.Log("CROMO DO BEST: " + _Rotas[Best].MostrarCromo());
        return Best;

    }

    int SelectSecondFit(int First)
    {
  
        int FirstBest = First;
        int SecondBest=0;
        MenorDist = float.MaxValue; //Var auxiliar

        for (int i = 0; i < _Rotas.Count; i++)
        {
            if (i != FirstBest)
            {
                if (_Rotas[i].CalculoDistRota() < MenorDist)
                {

                    MenorDist = _Rotas[i].CalculoDistRota();
                    SecondBest = i;

                }

            }

        }

        Debug.Log("CROMO DO SECOND BEST: " + _Rotas[SecondBest].MostrarCromo());
        return SecondBest;

    }

    void Crossover(int first, int second) // ARRUMAR
    {
        Rota Child = new Rota(this);
        Child.dna.Clear();
        Child.dna.Add(_Cidades[0]);

        bool aux = true;

        //Troca os valores dos genes
        for (int i = 1; i < qntCidades; i++)
        {            
            if (aux ==true)
            { 
                if(Child.VerificarGene(_Rotas[first], i) == false)
                {
                    Child.dna.Add(_Rotas[first].dna[i]);
                    aux = false;
                }
                else
                {
                    if (_Rotas[first].dna.ElementAtOrDefault(i+1)!= null)
                    {
                        Child.dna.Add(_Rotas[first].dna[i+1]);
                        aux = false;
                    }
                    else
                    {
                        Child.dna.Add(_Rotas[first].dna[i - 1]);
                        aux = false;
                    }

                }
              
            }
            else
            {
                if (Child.VerificarGene(_Rotas[second], i) == false)
                {
                    Child.dna.Add(_Rotas[second].dna[i]);
                    aux = true;
                }
                else
                {
                    if (_Rotas[second].dna.ElementAtOrDefault(i + 1) != null)
                    {
                        Child.dna.Add(_Rotas[second].dna[i + 1]);
                        aux = true;
                    }
                    else
                    {
                        Child.dna.Add(_Rotas[second].dna[i - 1]);
                        aux = true;
                    }

                }

            }
          
        }

        


        _Rotas.Add(Child);
        Debug.Log("CROMO DO FILHO: " + _Rotas[_Rotas.Count - 1].MostrarCromo());
  
    }

    //fazer metodo mutacao

    //Menos fit eh quem tem maior distancia percorrida
    void RetiraMenosFit()
    {        
        int menosfit = 0; //Recebe indice
        float MaiorDist=0;

        for (int i = 0; i < _Rotas.Count; i++)
        {        
            if( _Rotas[i].CalculoDistRota() > MaiorDist)
            {
                MaiorDist = _Rotas[i].CalculoDistRota();
                menosfit = i;
            }
        }

        Debug.Log("CROMO MENOS FIT: " + _Rotas[menosfit].MostrarCromo());

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

        _Cidades[0].SetOrigin(true); //Por convenção, cidade 0 é sempre origem
        _Cidades[0].GetComponent<MeshRenderer>().material = OriginMaterial; //Muda a cor da origem

        //Cria primeira população
        for (int i = 0; i < Populacao; i++)
        {            
            Rota Rota = new Rota(this);        
            _Rotas.Add(Rota);
        }


        //Mostra rota pop inicial
        for (int i = 0; i < _Rotas.Count; i++)
        {
            Debug.Log("Rota " + i + " tem cromo " + _Rotas[i].MostrarCromo() + " e distancia percorrida " + _Rotas[i].CalculoDistRota());
        }



    }

    private bool continuar = false;

    void Update()
    {
        //Não deixa iniciar simulação enquanto houver individuos iguais na primeira população.
        //Isso é pra evitar de ter um FirstBest e SecondBest com mesmo genes.
        if (continuar == false)
        {
            Debug.Log("Entrou continuar == false");
            for (int i = 0; i < _Rotas.Count; i++)
            {
                for (int j = 1; j < _Rotas.Count; j++)
                {
                    if (_Rotas[i].ComparaCromos(_Rotas[j]) == true)
                    {
                        _Rotas[i].dna.RemoveAt(0);
                        _Rotas[i].dna.Shuffle();
                        _Rotas[i].dna.Insert(0, _Cidades[0]);



                    }

                }

            }

            for (int i = 0; i < _Rotas.Count; i++)
            {
                for (int j = 1; j < _Rotas.Count; j++)
                {
                    if (_Rotas[i].ComparaCromos(_Rotas[j]) == true)
                    {

                        continuar = false;


                    }
                    else
                    {
                        continuar = true;
                        break;
                    }


                }

                if (continuar == true) break;
            }

        }
        else
        {
            Debug.Log("Entrou continuar == true");
            for (int i = 0; i < _Rotas.Count; i++)
            {
                Debug.Log("Rota " + i + " tem cromo " + _Rotas[i].MostrarCromo() + " e distancia percorrida " + _Rotas[i].CalculoDistRota());
            }

            if (Geracao < 300)
            {
                firstBest = SelectFirstFit();
               

                secondBest = SelectSecondFit(firstBest);


                Crossover(firstBest, secondBest);

                for (int i = 0; i < _Rotas.Count; i++)
                {
                    Debug.Log("Rota " + i + " tem cromo " + _Rotas[i].MostrarCromo() + " e distancia percorrida " + _Rotas[i].CalculoDistRota());
                }

                RetiraMenosFit();


                _Rotas[firstBest].DrawRoute();

                Geracao++;
            }

            if (Geracao == 300)
            {
                _Rotas[firstBest].DrawRoute();
                Debug.Log("O mais fit eh a rota " + firstBest + " de cromossomo "  + _Rotas[firstBest].MostrarCromo());
                UnityEditor.EditorApplication.Beep();
                UnityEditor.EditorApplication.isPaused = true;
            }

        }


    }


}
