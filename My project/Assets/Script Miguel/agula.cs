using UnityEngine;

public class Aguila : MonoBehaviour
{
    [Header("Ataque")]
    public float rangoDeVision = 6f;
    public float rangoDeAtaque = 1.5f;
    public float velocidad = 3f;
    public float dano = 25f;
    public float tiempoEntreAtaques = 2f;
    public LayerMask capaJugador;

    private Transform jugador;
    private float ultimoAtaque = -999f;
    private Rigidbody2D rb;
    private Vector3 posicionInicial;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeVision)
        {
            // Perseguir al jugador
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direccion.x * velocidad, direccion.y * velocidad);

            // Detectar si está en rango de ataque con OverlapCircle
            Collider2D jugadorDetectado = Physics2D.OverlapCircle(transform.position, rangoDeAtaque, capaJugador);
            if (jugadorDetectado != null && Time.time - ultimoAtaque >= tiempoEntreAtaques)
            {
                Configuracionvida vida = jugadorDetectado.GetComponent<Configuracionvida>();
                if (vida != null)
                {
                    vida.TomarDaño(dano);
                    ultimoAtaque = Time.time;
                    Debug.Log("¡Águila atacó!");
                }
            }
        }
        else
        {
            // Si no ve al jugador, se detiene
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeVision);
    }
}
