using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rota //Sem :Monobehavior pq se não eu não consigo referenciar pois ele não ta associado a nenhum gameobject
{   
    //tamanho array genes da rota tem q ser de acordo com numero de cidades+1
    //colocar calculo de fitness

    private Manager manager;
    private List<City> dna;

 

    public Rota(Manager manager)
    {
        this.manager = manager;
        dna = new List<City>(manager._Cidades); //Copia a lista de cidades do manager.
        dna.Add(manager._Cidades[Random.Range(0,manager._Cidades.Count)]); //Adicionei +1 cidade de indice random pq ele tem que sair da origem e voltar pra ela.
        dna.ShuffleList();
    }


    public void MostrarRota()
    {
        string aux=null;
    
        for (int i=0; i< dna.Count; i++)
        {
            aux += dna[i].GetID().ToString();
            
        }

        Debug.Log("O cromossomo dessa rota é: " + aux);
    }

    //Calcula dist de todas as cidades dessa rota.
    public float CalculoDistRota()
    {
        //calcula dist entre dna1-dna2, dna2-dna3 e assim por diante
        //ele soma tudo.
        float totDist = 0.0f;

        for(int i=0; i<dna.Count; i++)
        {
            for(int j=1; j<dna.Count; j++)
            {
                if(dna[j] != null)
                {
                    
                    totDist = totDist + dna[i].getCityDistance(dna[j].gameObject);           
    
                }

                i = j;
            }
        }


        return totDist;
    }


 

    public void DrawRoute()
    {
        for (int i = 0; i < dna.Count; i++)
        {
            for (int j = 1; j < dna.Count; j++)
            {
                if (dna[j] != null)
                {

                    dna[i].DrawLine(dna[j].gameObject);

                }
                i = j;
            }
        }
    }

 
}
