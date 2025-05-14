using UnityEngine;

public class OsoPatrulla : MonoBehaviour
{
    [Header("Configuración de Patrulla")]
    public Transform puntoInicio;    // Punto A de la patrulla
    public Transform puntoDestino;   // Punto B de la patrulla
    public float velocidad = 3f;     // Velocidad de movimiento
    public float distanciaCambio = 0.1f; // Distancia para cambiar de destino

    [Header("Configuración Visual")]
    public bool voltearSprite = true; // Si el sprite debe voltearse

    private Vector3 destinoActual;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtener componentes
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Verificar asignación de puntos
        if (puntoInicio == null || puntoDestino == null)
        {
            Debug.LogError("¡Error! Asigna ambos puntos de patrulla en el Inspector.");
            enabled = false; // Desactiva el script si faltan puntos
            return;
        }

        destinoActual = puntoDestino.position;
        Debug.Log($"Patrulla iniciada. Destino inicial: {destinoActual}");
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Movimiento con física (recomendado para 2D)
            Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, destinoActual, velocidad * Time.fixedDeltaTime);
            rb.MovePosition(nuevaPosicion);
        }
        else
        {
            // Movimiento sin física (para objetos estáticos)
            transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);
        }

        // Cambiar destino al llegar
        if (Vector3.Distance(transform.position, destinoActual) < distanciaCambio)
        {
            CambiarDestino();
        }

        // Voltear sprite si es necesario
        if (voltearSprite)
        {
            VoltearSprite();
        }
    }

    void CambiarDestino()
    {
        if (destinoActual == (Vector3)puntoDestino.position)
        {
            destinoActual = puntoInicio.position;
        }
        else
        {
            destinoActual = puntoDestino.position;
        }
        Debug.Log($"Nuevo destino: {destinoActual}");
    }

    void VoltearSprite()
    {
        // Usar SpriteRenderer si está disponible
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (destinoActual.x < transform.position.x);
        }
        else // Alternativa para objetos sin SpriteRenderer
        {
            if (destinoActual.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Dibuja gizmos en el editor para visualizar la ruta
    void OnDrawGizmosSelected()
    {
        if (puntoInicio != null && puntoDestino != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(puntoInicio.position, puntoDestino.position);
            Gizmos.DrawSphere(puntoInicio.position, 0.2f);
            Gizmos.DrawSphere(puntoDestino.position, 0.2f);
        }
    }
}