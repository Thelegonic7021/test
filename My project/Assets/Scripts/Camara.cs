using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.5f;
    public Vector3 desplazamiento;


    public void LateUpdate()
    {
        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        transform.position = posicionSuavizada;
    }

}
