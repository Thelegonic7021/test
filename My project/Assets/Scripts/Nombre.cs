using System;
using UnityEngine;

public class Nombre : MonoBehaviour
{
    String names;

    public void ReadNameStrings(string name){
<<<<<<< HEAD
        names = name;
        Debug.Log(names);
    }

    public void Destruir(string name)
    {
=======
    names = name;
    PlayerPrefs.SetString("NombreJugador", names); // <<< guarda el nombre
    PlayerPrefs.Save(); // <<< asegura que se guarde inmediatamente
    Debug.Log("Nombre guardado: " + names);
    }


    public void Destruir(string name){
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
        Destroy(gameObject);
        GameObject mensaje = GameObject.FindWithTag("Mensaje");
        Destroy(mensaje);

    }
}
