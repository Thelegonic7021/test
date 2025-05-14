using UnityEngine;

public class CamaraControl : MonoBehaviour
{
    public Transform objetivo;            // Objeto a seguir (por ejemplo, el jugador)
    public float velocidadCamara = 5f;    // Velocidad de seguimiento (ajustable)
    public Vector3 desplazamiento;        // Offset respecto al objetivo

    private float duracionSacudida = 0f;
    public float intensidadSacudida = 0.1f;
    public float tiempoSacudida = 1f;

    public Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Calcula la posición deseada con desplazamiento
        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        // Mantiene la posición Z original de la cámara (importante en 2D)
        posicionDeseada.z = transform.position.z;

        // Movimiento suave de la cámara
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara * Time.deltaTime);
        transform.position = posicionSuavizada;

        // Efecto de sacudida de cámara
        if (duracionSacudida > 0)
        {
            transform.position += new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0) * intensidadSacudida;
            duracionSacudida -= Time.deltaTime;
        }
    }

    // Método público para activar sacudida
    public void EmpezarSacudida()
    {
        duracionSacudida = tiempoSacudida;
    }
}
