using UnityEngine;

public class Configuracionvida : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float maximaVida = 100f;
    [SerializeField] private BarraDeVida barraDeVida;

    private Vector3 posicionInicial;

    private void Start()
    {
        vida = maximaVida;
        barraDeVida?.InicializarBarraDeVida(maximaVida);
        posicionInicial = transform.position;
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;
        Debug.Log("El jugador recibió daño! Vida actual: " + vida);

        if (barraDeVida != null)
        {
            barraDeVida.CambiarVidaActual(vida);
        }

        if (vida <= 0)
        {
            ReiniciarJugador();
        }
    }

    private void ReiniciarJugador()
    {
        transform.position = posicionInicial;
        vida = maximaVida;
        barraDeVida?.InicializarBarraDeVida(maximaVida);
    }
}
