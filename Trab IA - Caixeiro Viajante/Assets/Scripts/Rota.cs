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
        dna.Add(manager._Cidades[Random.Range(0,manager._Cidades.Count)]); //Adicionei +1 cidade pq ele tem que sair da origem e voltar pra ela.
    }


    public void MostrarRota()
    {
        Debug.Log("O cromossomo dessa rota é: ");
        for(int i=0; i< dna.Count; i++)
        {
           dna[i].GetID();
        }
    }

    //Calcula dist de todas as cidades dessa rota.
    public float CalculoDistRota()
    {
        float totDist = 0.0f;

        for(int i=0; i<dna.Count; i++)
        {
            for(int j=1; j<dna.Count; j++)
            {
                if(dna[j] != null)
                {
                    
                    totDist+=dna[i].getCityDistance(dna[j].gameObject);
                }
            }
        }


        return totDist;
    }


 

    public void ShowRouteInfo()
    {

    }

 
}
