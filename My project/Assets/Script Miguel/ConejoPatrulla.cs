using UnityEngine;

public class BunnyMover : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public float distanciaMax = 3f;

    [Header("Daño por contacto")]
    public float contactDamage = 1f;
    private float lastDamageTime;
    public float damageCooldown = 1f;

    [Header("Vida del conejo")]
    public int vidaMaxima = 3;
    private int vidaActual;

    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private bool yendoDerecha = true;
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = rb.position;
        escalaOriginal = transform.localScale;
        vidaActual = vidaMaxima;

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void FixedUpdate()
    {
        Vector2 direccion = yendoDerecha ? Vector2.right : Vector2.left;
        rb.MovePosition(rb.position + direccion * speed * Time.fixedDeltaTime);

        if (Vector2.Distance(rb.position, posicionInicial) >= distanciaMax)
        {
            yendoDerecha = !yendoDerecha;
            transform.localScale = new Vector3(
                yendoDerecha ? escalaOriginal.x : -escalaOriginal.x,
                escalaOriginal.y,
                escalaOriginal.z
            );
            posicionInicial = rb.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            Configuracionvida playerHealth = collision.gameObject.GetComponent<Configuracionvida>();
            if (playerHealth != null)
            {
                playerHealth.TomarDaño(contactDamage);
                lastDamageTime = Time.time;
                Debug.Log("El conejo dañó al jugador.");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            vidaActual--;
            Debug.Log("Conejo recibió daño. Vida restante: " + vidaActual);

            if (vidaActual <= 0)
            {
                Debug.Log("¡Conejo eliminado!");
                Destroy(gameObject);
            }
        }
    }
}
