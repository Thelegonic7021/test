using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ajustar_volumen : MonoBehaviour
{
    public Slider sliderVolumen;
    public TextMeshProUGUI textoPorcentaje;
    public static float volume {get;set;}
    void Update()
    {
        float valorActual = sliderVolumen.value;
        if(textoPorcentaje != null){
            textoPorcentaje.text = (valorActual).ToString("0.0");
            AudioListener.volume = valorActual;
        }
    }
    
}
