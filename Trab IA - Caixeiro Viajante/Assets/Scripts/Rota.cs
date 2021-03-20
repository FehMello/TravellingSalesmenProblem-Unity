using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rota : MonoBehaviour
{
    //tamanho array genes da rota tem q ser de acordo com numero de cidades+1
    //colocar calculo de fitness
    private int _ID;

    private Manager manager;
    private List<City> dna;

 

    public Rota(int _ID, Manager manager)
    {
        this.manager = manager;
        this._ID = _ID;
        dna = new List<City>(manager._Cidades);
    }

 
    //public void CriaCromossomosIniciais(int numerocidades, City c)
    //{
        
    //    int tempCidades = numerocidades;
    //    numerocidades++;

    //    for(int i=0; i<numerocidades; i++)
    //    {
    //        Cromos.Add(Random.Range(0, tempCidades));
    //    }

    //}

 
}
