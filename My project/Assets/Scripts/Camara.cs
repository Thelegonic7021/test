using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.5f;
    public Vector3 desplazamiento;

    private float duracionSacudida = 0f;
    public float intensidadSacudida = 0.1f; // Qué tanto se sacude
    public float tiempoSacudida = 1f;     // Cuánto dura la sacudida

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 posicionDeseada = objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);
        transform.position = posicionSuavizada;

        if (duracionSacudida > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle * intensidadSacudida;
            duracionSacudida -= Time.deltaTime;
        }
    }

    public void EmpezarSacudida()
    {
        duracionSacudida = tiempoSacudida;
    }
}
