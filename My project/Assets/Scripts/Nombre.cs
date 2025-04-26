using System;
using UnityEngine;

public class Nombre : MonoBehaviour
{
    String names;

    public void ReadNameStrings(string name){
        names = name;
        Debug.Log(names);
    }

    public void Destruir(string name)
    {
        Destroy(gameObject);
        GameObject mensaje = GameObject.FindWithTag("Mensaje");
        Destroy(mensaje);

    }
}
