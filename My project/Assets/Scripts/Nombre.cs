using System;
using UnityEngine;

public class Nombre : MonoBehaviour
{
    String names;

    public void ReadNameStrings(string name){
        names = name;
        PlayerPrefs.SetString("NombreJugador", names); // Guarda el nombre
        PlayerPrefs.Save(); // Asegura que se guarde inmediatamente
        Debug.Log("Nombre guardado: " + names);
    }

    public void Destruir(string name){
        Destroy(gameObject);
        GameObject mensaje = GameObject.FindWithTag("Mensaje");
        Destroy(mensaje);
    }
}