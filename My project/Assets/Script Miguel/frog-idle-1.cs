using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float range = 3f;

    [Header("Damage Settings")]
    public float contactDamage = 1f; // La cantidad de daño que inflige
    private float lastDamageTime;
    public float damageCooldown = 1f; // Tiempo entre cada daño

    private Vector2 startPoint;
    private bool movingRight = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPoint = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initial size adjustment
        transform.localScale *= 0.7f;
    }

    void Update()
    {
        PatrolMovement();
    }

    void PatrolMovement()
    {
        // Movement
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);

        // Sprite flipping
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = movingRight;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (movingRight ? -1 : 1);
            transform.localScale = scale;
        }

        // Direction change
        if (Vector2.Distance(transform.position, startPoint) >= range)
        {
            movingRight = !movingRight;
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
            }
        }
    }

    // Ya no necesitamos OnTriggerEnter2D ni ApplyKnockback para quitar vida directamente por contacto
    // Si aún quieres aplicar knockback, podrías mover esa lógica a OnCollisionEnter2D
}