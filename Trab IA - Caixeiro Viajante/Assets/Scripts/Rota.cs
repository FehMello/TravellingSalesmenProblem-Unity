using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rota //Sem :Monobehavior pq se não eu não consigo referenciar pois ele não ta associado a nenhum gameobject
{   
    //tamanho array genes da rota tem q ser de acordo com numero de cidades+1
    //colocar calculo de fitness

    private Manager manager;
    public List<City> dna;
 
    //Convenção: todas as rotas vão começar no 0
    public Rota(Manager manager)
    {
        this.manager = manager;
        dna = new List<City>(manager._Cidades); //Copia a lista de cidades do manager.
        dna.Remove(dna[0]); // Remove a cidade 0 que ta na posicao 0
        dna.Shuffle(); // Mistura
        dna.Insert(0, manager._Cidades[0]); //Insere cidade 0 na posicao 0 novamente
    }


    public string MostrarCromo()
    {
        string aux=null;
    
        for (int i=0; i< dna.Count; i++)
        {
            aux += dna[i].GetID().ToString();
            
        }

        return aux;
    }

    //Calcula dist percorrida na rota.
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
                    
                    totDist = totDist + dna[i].getCityDistance(dna[j].gameObject); //Da origem até ultimo    
    
                }

                i = j;
            }
        }

        totDist = totDist + dna[dna.Count - 1].getCityDistance(dna[0].gameObject); //Do ultimo DIRETO até origem (sem passar de novo nas outras city)


        return totDist;
    }

    //Compara se o cromossomo inteiro de um individuo eh igual ao outro
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

    //Verifica se o gene especifico do parente em questão já existe no cromo do filho
    public bool VerificarGene(Rota rota, int index)  
    {
        bool repetido = false;

        if (this.dna.Count == 1) //Verifica se este dna (que puxou a função) tá vazio
        {
            //Aqui significa que child só tem a origem por enquanto
            repetido = false;
        }
        else
        {
            for (int i = 1; i < this.dna.Count; i++)
            {
                if(rota.dna[index].GetID() == this.dna[i].GetID()) //Compara dna especifico com todos q tiver no child atualmente
                {
                    repetido = true; // eh necessario apenas 1 igual para que seja true
                    break;
                }

            }
        }

        return repetido;
    }



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

        dna[dna.Count - 1].DrawLine(dna[0].gameObject); //Do ultimo DIRETO até origem (sem passar de novo nas outras city)
    }

 
}
