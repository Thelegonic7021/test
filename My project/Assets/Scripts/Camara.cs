using UnityEngine;

public class Camara1_pablo: MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.5f;
    public Vector3 desplazamiento;

    private float duracionSacudida = 0f;
    public float intensidadSacudida = 0.1f; 
    public float tiempoSacudida = 1f;     

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

    // Solo una versión del método
    public void EmpezarSacudida()
    {
        duracionSacudida = tiempoSacudida;
    }
}