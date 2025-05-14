using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        // Verifica si el slider no fue asignado manualmente
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();

            if (slider == null)
            {
                Debug.LogError("❌ ERROR: No se encontró el Slider en BarraDeVida.");
            }
        }
    }

    public void InicializarBarraDeVida(float cantidadVida)
    {
        if (slider != null)
        {
            slider.maxValue = cantidadVida;
            slider.value = cantidadVida;
        }
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        if (slider != null)
        {
            slider.value = cantidadVida;
        }
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        if (slider != null)
        {
            slider.maxValue = vidaMaxima;
        }
    }
}
