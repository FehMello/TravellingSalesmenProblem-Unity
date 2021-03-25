using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TMP_Text Geracaotext;
    public TMP_Text Besttext;
    public Manager Manager;


    public void AtualizaGeracao(int geracao)
    {
        Geracaotext.SetText("Geração: " + geracao);
    }

    public void AtualizaBest(Rota rota)
    {
        Besttext.gameObject.SetActive(true);
        Besttext.SetText("Melhor cromossomo: " + rota.MostrarCromo());
    }

    void Start()
    {
        Besttext.gameObject.SetActive(false);
        Manager=Manager.FindObjectOfType<Manager>();
        
    }


}
