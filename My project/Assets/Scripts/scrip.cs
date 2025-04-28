using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrip : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float maximaVida;
    [SerializeField] private BarraDeVida barraDeVida;

    private void Start()
    {
        vida = maximaVida;
        barraDeVida.InicializarBarraDeVida(vida);
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;
        barraDeVida.CambiarVidaActual(vida);
        if (vida <= 0)
        {
            ReiniciarJugador();
        }
    }
    private void ReiniciarJugador()
    {
        // Define aquí la posición donde quieres que reaparezca
        transform.position = new Vector3(0f, 0f, 0f); // Ejemplo: (0,0,0)
        vida = maximaVida;
        barraDeVida.InicializarBarraDeVida(vida);
    }
}
