using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class City : MonoBehaviour
{
    public TMP_Text IdText;

    private int _id;
    private bool _isOrigin = false;
    private LineRenderer _renderer;

    void Start()
    {
        _renderer = transform.GetComponent<LineRenderer>();
    }

    public void SetID(int value)
    {
        _id = value;
    }

    public int GetID()
    {
        return _id;
    }

    public void SetText(int value)
    {
        IdText.SetText("CITY "+ value.ToString());
        
    }

    public void SetOrigin(bool value)
    {
        _isOrigin = value;
    }

    public bool GetOrigin()
    {
        return _isOrigin;
    }

    //Retorna distância da cidade atual até a cidade "other"
    public float GetCityDistance(GameObject other) 
    {
        float cityDist = 0.0f;

        cityDist = Vector3.Distance(transform.position, other.transform.position);

        return cityDist;
    }

    //Desenha a linha entre a cidade atual até a cidade "other"
    public void DrawLine(GameObject other)
    {
        _renderer.SetPosition(0, this.transform.position);
        _renderer.SetPosition(1, other.transform.position);
    }

}
