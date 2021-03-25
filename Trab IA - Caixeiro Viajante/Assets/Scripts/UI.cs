using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TMP_Text Geracaotext;
    public TMP_Text Besttext;
    public Manager Manager;

  

    // Start is called before the first frame update
    void Start()
    {
        Manager=Manager.FindObjectOfType<Manager>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
        Geracaotext.SetText("Geração: " + Manager.Geracao);

        Besttext.SetText("Melhor cromossomo: " + Manager._Rotas[Manager.firstBest].MostrarCromo());
   
        
    }
}
