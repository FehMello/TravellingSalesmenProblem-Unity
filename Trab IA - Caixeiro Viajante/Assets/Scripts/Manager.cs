using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class Manager : MonoBehaviour
{
    public Material OriginMaterial;
    public int QntCidades;
    public GameObject CityObj; 
    public int Populacao = 0; //Numero de indivíduos(ROTAS) a serem criadas. É especificado no inspector e NÃO deve ser alterado durante o código.
    public int LimiteGeracoes;
    [System.NonSerialized]
    public int Geracao = 0;
    [System.NonSerialized]
    public List <Rota> Rota = new List<Rota>(); //O tamanho dessa lista tem que se manter igual ao número de população durante cada geração.
    [System.NonSerialized]
    public List<City> Cidade = new List<City>(); 

    private int _firstBest = 0;
    private int _secondBest = 0;
    private float _menorDist;
    private bool _continuar;
    private UI _uiScript;
    private City _cityScript;
    private float _chance;


    void Start()
    {
        _continuar = false;
        _uiScript = UI.FindObjectOfType<UI>();

        //Instancia cidades em posições aleatórias e adiciona na lista
        for (int i = 0; i < QntCidades; i++)
        {
            GameObject CityTMP;
            Vector3 position = new Vector3(Random.Range(-13.0F, 13.0F), 0, Random.Range(-13.0F, 13.0F));
            CityTMP=Instantiate(CityObj, position, Quaternion.identity) as GameObject; //Cria um novo gameobject baseado no prefab CityObj.
            _cityScript = CityTMP.GetComponent<City>(); //Para não precisar repetir getcomponent durante o restante do script, pois é pesado
            Cidade.Add(_cityScript);
        }

        //Associa IDs às cidades e coloca os nomes correspondentes
        for (int i = 0; i < QntCidades; i++)
        {
            Cidade[i].SetID(i);
            Cidade[i].SetText(Cidade[i].GetID());
         
        }

        Cidade[0].SetOrigin(true); //Por convenção, cidade 0 é sempre origem
        Cidade[0].GetComponent<MeshRenderer>().material = OriginMaterial; //Muda a cor da origem

        //Cria primeira população
        for (int i = 0; i < Populacao; i++)
        {            
            Rota rota = new Rota(this);
            Rota.Add(rota);
        }

        //Mostra rota pop inicial para fins de DEBUG
        for (int i = 0; i < Rota.Count; i++)
        {
            Debug.Log("Rota " + i + " tem cromo " + Rota[i].MostrarCromo() + " e distancia percorrida " + Rota[i].CalculoDistRota());
        }
    }

    void Update()
    {
        //Não deixa iniciar simulação enquanto houver individuos iguais na primeira população.
        //Isso é pra evitar de ter um FirstBest e SecondBest com mesmo genes.
        if (_continuar == false)
        {
            Debug.Log("Entrou continuar == false");

            for (int i = 0; i < Rota.Count; i++)
            {
                for (int j = 1; j < Rota.Count; j++)
                {
                    if (Rota[i].ComparaCromos(Rota[j]) == true)
                    {
                        Rota[i].dna.RemoveAt(0);
                        Rota[i].dna.Shuffle();
                        Rota[i].dna.Insert(0, Cidade[0]);
                    }

                }

            }

            for (int i = 0; i < Rota.Count; i++)
            {
                for (int j = 1; j < Rota.Count; j++)
                {
                    if (Rota[i].ComparaCromos(Rota[j]) == true)
                    {

                        _continuar = false;


                    }
                    else
                    {
                        _continuar = true;
                        break;
                    }
                }

                if (_continuar == true) break;
            }

        }
        else
        {
            Debug.Log("Entrou continuar == true");

            for (int i = 0; i < Rota.Count; i++)
            {
                Debug.Log("Rota " + i + " tem cromo " + Rota[i].MostrarCromo() + " e distancia percorrida " + Rota[i].CalculoDistRota());
            }

            if (Geracao < LimiteGeracoes)
            {                              
                _firstBest = SelectFirstFit();


                _secondBest = SelectSecondFit(_firstBest);


                Crossover(_firstBest, _secondBest);

                _chance = Random.Range(0f, 1.0f);

                Debug.Log("CHANCE: " + _chance);

                if (_chance > 0.8) //20% chance de mutação
                {
                    Mutacao();
                }
               

                //for (int i = 0; i < Rota.Count; i++)
                //{
                //    Debug.Log("Rota " + i + " tem cromo " + Rota[i].MostrarCromo() + " e distancia percorrida " + Rota[i].CalculoDistRota());
                //}

                RetiraMenosFit();


                Rota[_firstBest].DrawRoute();

                Geracao++;
                _uiScript.AtualizaGeracao(Geracao);
                
            }

            if (Geracao == LimiteGeracoes)
            {
                _uiScript.AtualizaGeracao(Geracao);
                _uiScript.AtualizaBest(Rota[_firstBest]);
                Rota[_firstBest].DrawRoute();
                Debug.Log("O mais fit é a Rota " + _firstBest + " de cromossomo "  + Rota[_firstBest].MostrarCromo());
                UnityEditor.EditorApplication.Beep();
                UnityEditor.EditorApplication.isPaused = true;
            }

        }


    }

    //Seleciona o mais fitness
    int SelectFirstFit()
    {

        int bestIndex = 0;

        _menorDist = float.MaxValue;

        for (int i = 0; i < Rota.Count; i++)
        {
            if (Rota[i].CalculoDistRota() < _menorDist)
            {

                _menorDist = Rota[i].CalculoDistRota();
                bestIndex = i;

            }

        }

        Debug.Log("CROMO DO BEST: " + Rota[bestIndex].MostrarCromo());
        return bestIndex;

    }

    //Seleciona o segundo mais fitness
    int SelectSecondFit(int first)
    {

        int firstBest = first;
        int secondBest = 0;

        _menorDist = float.MaxValue;

        for (int i = 0; i < Rota.Count; i++)
        {
            if (i != firstBest)
            {
                if (Rota[i].CalculoDistRota() < _menorDist)
                {

                    _menorDist = Rota[i].CalculoDistRota();
                    secondBest = i;

                }

            }

        }

        Debug.Log("CROMO DO SECOND BEST: " + Rota[secondBest].MostrarCromo());
        return secondBest;

    }

    //Crossover usando Ordered Crossover (OX1) - É mais adequado quando não queremos repetir elemento
    void Crossover(int first, int second)
    {
        //Remove o primeiro elemento dos parents pra ficar mais simples de iterar.
        Rota[first].dna.RemoveAt(0);
        Rota[second].dna.RemoveAt(0);

        Rota Child = new Rota(this);
        Child.dna.Clear();

        int Startcutindex = 0;
        int Endcutindex = 0;
        Startcutindex = Random.Range(0, Rota[first].dna.Count - 1);
        Endcutindex = Random.Range(Startcutindex + 1, Rota[first].dna.Count - 1);
        Debug.Log("StartCut: " + Startcutindex + " || Endcut: " + Endcutindex);

        var slice = Rota[first].dna.Skip(Startcutindex).Take(Endcutindex - Startcutindex).ToList(); //Pega um trecho random do best.
        //Skip = IGNORA o numero de elementos especificados no parenteses e RETORNA o restante de elementos depois desses elementos pulados
        //Take = RETORNA os primeiros x especificados no parenteses e IGNORA o restante.

        //Pega o segundo melhor e preenche o child com o que falta de dna
        for (int i = 0; i < Rota[second].dna.Count; i++)
        {
            if (!slice.Contains(Rota[second].dna[i]))
            {
                slice.Add(Rota[second].dna[i]);
            }
        }

        Child.dna = slice;
        Child.dna.Insert(0, Cidade[0]);
        Rota[first].dna.Insert(0, Cidade[0]);
        Rota[second].dna.Insert(0, Cidade[0]);


        Rota.Add(Child);

        Debug.Log("CROMO DO FILHO: " + Rota[Rota.Count - 1].MostrarCromo());

    }

    //Realiza mutação de uma rota randomicamente escolhida
    void Mutacao()
    {
        int randomRota = 0;
        int randomPos1 = 0;
        int randomPos2 = 0;
        City cityTemp;

        randomRota = Random.Range(0, Rota.Count - 1);

        Debug.Log("MUTAÇÃO: Cidade escolhida foi de cromo " + Rota[randomRota].MostrarCromo());
        Rota[randomRota].dna.RemoveAt(0);

        randomPos1 = Random.Range(0, Rota[randomRota].dna.Count - 1);
        randomPos2 = Random.Range(0, Rota[randomRota].dna.Count - 1);

        if (randomPos1 == randomPos2)
        {
           randomPos2 = Random.Range(0, Rota[randomRota].dna.Count - 1);
        }
       
        cityTemp = Rota[randomRota].dna[randomPos1];
        Rota[randomRota].dna[randomPos1] = Rota[randomRota].dna[randomPos2];
        Rota[randomRota].dna[randomPos2] = cityTemp;

        Rota[randomRota].dna.Insert(0, Cidade[0]);

        Debug.Log("MUTAÇÃO: Cidade após mutação " + Rota[randomRota].MostrarCromo());
    }

    //Menos fit eh quem tem maior distancia percorrida
    void RetiraMenosFit()
    {
        int menosfit = 0; //Recebe indice
        float MaiorDist = 0;

        for (int i = 0; i < Rota.Count; i++)
        {
            if (Rota[i].CalculoDistRota() > MaiorDist)
            {
                MaiorDist = Rota[i].CalculoDistRota();
                menosfit = i;
            }
        }

        Debug.Log("CROMO MENOS FIT: " + Rota[menosfit].MostrarCromo());

        Rota.RemoveAt(menosfit);
    }
}
