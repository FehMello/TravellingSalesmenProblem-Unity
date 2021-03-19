using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class City : MonoBehaviour
{
    //Nao colocar isso publico se nao a unity fica considerando o valor do inspector so grr
    [SerializeField]
    private int ID; //ID que vai ser usada no cromossomo do individuo rota
    private bool isOrigin = false; // Diz se essa cidade é a origem, lembrando que a rota tem que sair da origem e chegar na origem.
    [SerializeField]
    private TMP_Text IDtext;

    void Start()
    {
        IDtext = transform.GetComponentInChildren<TMP_Text>();
    }

    public void SetID(int value)
    {
        ID = value;
    }

    public int GetID()
    {
        Debug.Log("This city ID is " + ID);
        return ID;
    }

    public void SetText(int value)
    {
        IDtext.SetText("CITY "+value.ToString());
        
    }



}
