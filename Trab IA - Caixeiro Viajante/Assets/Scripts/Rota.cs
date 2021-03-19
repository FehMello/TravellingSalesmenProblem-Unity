using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rota : MonoBehaviour
{
    //tamanho array genes da rota tem q ser de acordo com numero de cidades+1
    //

    Queue Cromos = new Queue();

    // Start is called before the first frame update
    public void CriaCromossomosIniciais(int numerocidades)
    {
        
        int tempCidades = numerocidades;
        numerocidades++;

        for(int i=0; i<numerocidades; i++)
        {
            Cromos.Enqueue(Random.Range(0, tempCidades));
        }

    }
}
