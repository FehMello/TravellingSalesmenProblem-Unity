using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //nossa população/individuos q estamos testando n sao as cidades e sim as rotas
    //as rotas (individuos) terao a cidade no array.
    public Material OriginMaterial;
    public int qntCidades;//qntCidades == qntGenes
    public GameObject CityObj; //Prefab da cidade
    public int Populacao = 0; //Numero de individuos(ROTAS) a serem criadas, esse numero é especificado no inspector e não deve ser alterado no código.
    public int Geracao = 0;
    public int firstBest = 0;
    public int secondBest = 0;


    private List <Rota> _Rotas = new List<Rota>(); //O tamanho dessa lista tem que se manter igual à qnt de população durante o programa todo.
    public List<City> _Cidades = new List<City>();//Pra ter controle das cidades
    private City _CityScript;
    private float MenorDist; //Variavel modifica dentro de mais de um metodo

    int SelectFirstFit() //ok
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

    int SelectSecondFit(int First)//ok
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

    //usar ordered crossover (ox1) - mais adequado quando se pode ter um elem de cada vez 
    void Crossover(int first, int second)
    {
        //Remove o primeiro elem dos parents pra ficar mais simples de iterar.
        _Rotas[first].dna.RemoveAt(0); 
        _Rotas[second].dna.RemoveAt(0);
        
        Rota Child = new Rota(this);
        Child.dna.Clear();

        int Startcutindex = 0;
        int Endcutindex = 0;
        Startcutindex = Random.Range(0, _Rotas[first].dna.Count-1);
        Endcutindex = Random.Range(Startcutindex+1, _Rotas[first].dna.Count - 1);
        Debug.Log("StartCut: " + Startcutindex + " || Endcut: " + Endcutindex);

        var slice = _Rotas[first].dna.Skip(Startcutindex).Take(Endcutindex - Startcutindex).ToList(); //Pega um trecho random do best.
        //Skip = IGNORA o numero de elementos especificados no parenteses e RETORNA o restante de elementos depois desses elementos pulados
        //Take = RETORNA os primeiros x especificados no parenteses e IGNORA o restante.


        for (int i = 0; i < _Rotas[second].dna.Count; i++) //Pega do second o que falta de dna
        {
            if (!slice.Contains(_Rotas[second].dna[i]))
            {
                slice.Add(_Rotas[second].dna[i]);
            }
        }

        Child.dna = slice;
        Child.dna.Insert(0, _Cidades[0]);
        _Rotas[first].dna.Insert(0, _Cidades[0]);
        _Rotas[second].dna.Insert(0, _Cidades[0]);


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
