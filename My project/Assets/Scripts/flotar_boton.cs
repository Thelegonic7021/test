using UnityEngine;

public class flotar_boton : MonoBehaviour
{
    public float altura = 0.5f;  // Altura del movimiento
    public float velocidad = 1.5f; // Velocidad del movimiento

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        float nuevaY = Mathf.Sin(Time.time * Mathf.PI*2* velocidad) * altura;
        transform.localPosition = posicionInicial + new Vector3(0, nuevaY, 0);
    }
}
