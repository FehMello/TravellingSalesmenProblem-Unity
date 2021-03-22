using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class City : MonoBehaviour
{
    //Nao colocar isso publico se nao a unity fica considerando o valor do inspector so grr
    [SerializeField]
    private int _ID; //ID que vai ser usada no cromossomo do individuo rota
    private bool _isOrigin = false; // Diz se essa cidade é a origem, lembrando que a rota tem que sair da origem e chegar na origem.
    [SerializeField]
    private TMP_Text _IDtext;

    void Start()
    {
        _IDtext = transform.GetComponentInChildren<TMP_Text>();
    }

    public void SetID(int value)
    {
        _ID = value;
    }

    public int GetID()
    {
        Debug.Log("This city ID is " + _ID);
        return _ID;
    }

    public void SetText(int value)
    {
        _IDtext.SetText("CITY "+value.ToString());
        
    }

    public float getCityDistance(GameObject other)
    {
        float cityDist = 0f;

        cityDist=Vector3.Distance(transform.position, other.transform.position);

        return cityDist;
    }



}
