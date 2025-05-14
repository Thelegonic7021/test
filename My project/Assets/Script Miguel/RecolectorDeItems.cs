using UnityEngine;
using TMPro;
using System.Collections;

public class RecolectorDeItems : MonoBehaviour
{
    public int cantidadItems = 0;
    public TextMeshProUGUI textoItems;
    public TextMeshProUGUI mensajeTexto; // Texto para mostrar mensajes temporales

    void Start()
    {
        ActualizarTexto();
        if (mensajeTexto != null)
            mensajeTexto.text = "";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            cantidadItems++;
            ActualizarTexto();
            Destroy(collision.gameObject);
        }
    }

    public void QuitarItems(int cantidad)
    {
        if (cantidadItems >= cantidad)
        {
            cantidadItems -= cantidad;
            ActualizarTexto();
            MostrarMensajeTemporal("Gracias por tu ayuda!");
        }
        else
        {
            MostrarMensajeTemporal("No tienes suficientes ítems.");
        }
    }

    void ActualizarTexto()
    {
        if (textoItems != null)
            textoItems.text = "Items: " + cantidadItems;
    }

    void MostrarMensajeTemporal(string mensaje)
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.text = mensaje;
            StopAllCoroutines(); // Por si ya hay uno en curso
            StartCoroutine(DesaparecerMensaje());
        }
    }

    IEnumerator DesaparecerMensaje()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos
        if (mensajeTexto != null)
            mensajeTexto.text = "";
    }
}
