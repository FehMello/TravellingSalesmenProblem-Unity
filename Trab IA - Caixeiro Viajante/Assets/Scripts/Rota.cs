using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Sem Monobehavior, pois eu não consigo referencia-lo, já que ele não está associado a nenhum gameObject na Scene.
public class Rota 
{   
    private Manager _manager;
    public List<City> dna;
 
    //Convenção: todas as rotas vão começar na Cidade 0
    public Rota(Manager manager)
    {
        this._manager = manager;
        dna = new List<City>(manager.Cidade); //Copia a lista de cidades do Manager.
        dna.Remove(dna[0]); //Remove a Cidade 0 que está na posição 0
        dna.Shuffle(); //Mistura
        dna.Insert(0, manager.Cidade[0]); //Insere Cidade 0 na posição 0 novamente
    }

    //Mostra a sequência de dna (Cidades)
    public string MostrarCromo()
    {
        string aux=null;
    
        for (int i=0; i< dna.Count; i++)
        {
            aux += dna[i].GetID().ToString();
            
        }

        return aux;
    }

    //Calcula distância percorrida pela rota
    public float CalculoDistRota()
    {
        //Calcula dist entre dna1-dna2, dna2-dna3 e assim por diante

        float totDist = 0.0f;

        for(int i=0; i<dna.Count; i++)
        {
            for(int j=1; j<dna.Count; j++)
            {
                if(dna[j] != null)
                {
                    
                    totDist = totDist + dna[i].GetCityDistance(dna[j].gameObject); //Da origem até ultimo    
    
                }

                i = j;
            }
        }

        totDist = totDist + dna[dna.Count - 1].GetCityDistance(dna[0].gameObject); //Do ultimo DIRETO até origem (sem passar de novo nas outras city)


        return totDist;
    }

    //Compara se o cromossomo inteiro de um individuo é igual ao outro
    public bool ComparaCromos(Rota rota) 
    {
        bool igual = false;

        for(int i=0; i<this.dna.Count; i++)
        {
            if(this.dna[i].GetID() == rota.dna[i].GetID())
            {
                igual = true;
            }
            else
            {
                igual = false;
                break;
            }
            if (igual == false) break;
        }

        return igual;

    }

    //Desenha as linhas conforme a ordem do dna
    public void DrawRoute()
    {
        for (int i = 0; i < dna.Count; i++)
        {
            for (int j = 1; j < dna.Count; j++)
            {
                if (dna[j] != null)
                {

                    dna[i].DrawLine(dna[j].gameObject); //Da origem até ultimo  

                }
                i = j;
            }
        }

        dna[dna.Count - 1].DrawLine(dna[0].gameObject); //Do ultimo DIRETO até origem (sem passar de novo nas outras City)
    }

 
}
