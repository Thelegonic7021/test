using UnityEngine;
using UnityEngine.SceneManagement;

public class ProyectilReciclable : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoDeVidaMaximo = 8.0f;
    public float velocidadInicial = 15.0f;
    public float velocidadDevolución = 20.0f;

    [Header("Efectos")]
    public GameObject efectoDevolucion;
    public GameObject efectoImpacto;
    
    private bool hasBeenReturned = false;
    private GameObject torreOrigen;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    
    void Start()
    {
        // Autodestrucción tras X segundos
        Destroy(gameObject, tiempoDeVidaMaximo);
    }
    
    public void SetTorreOrigen(GameObject torre)
    {
        torreOrigen = torre;
    }
    
    // Método llamado por el parry del jugador
    public void Devolver(Vector2 direccion)
    {
        if (hasBeenReturned) return;
        
        hasBeenReturned = true;
        Debug.Log("¡Proyectil reciclable devuelto!");
        
        if (efectoDevolucion != null)
            Instantiate(efectoDevolucion, transform.position, Quaternion.identity);
        
        // Cambiar color para indicar que fue devuelto
        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;
        
        // Cambiar color de trail si existe
        if (trailRenderer != null)
            trailRenderer.startColor = Color.green;
        
        // Aplicar velocidad en la nueva dirección
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direccion * velocidadDevolución;
            
            // Ignorar colisiones con el jugador tras ser devuelto
            Physics2D.IgnoreLayerCollision(
                gameObject.layer, 
                LayerMask.NameToLayer("Player"), 
                true
            );
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenReturned)
        {
            // Solo dañar si el jugador no está en modo inmunidad
            JohnMovement jugador = other.GetComponent<JohnMovement>();
            if (jugador != null && !jugador.esInmune)
            {
                Debug.Log("¡Jugador golpeado por proyectil reciclable! Iniciando respawn...");
                
                // Usar la misma lógica que en Proyectil.cs
                DeathZone deathZone = FindFirstObjectByType<DeathZone>();
                if (deathZone != null)
                {
                    deathZone.StartCoroutine(deathZone.ProcesarMuerte(other.gameObject));
                }
                else
                {
                    Debug.LogError("No hay DeathZone en la escena. Recargando como fallback.");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            
            // Destruir siempre al tocar jugador
            Destroy(gameObject);
        }
        else if (other.CompareTag("Torre") && hasBeenReturned)
        {
            Debug.Log("¡Proyectil reciclable impactó en torre!");
            
            // Si golpeamos a la torre de origen, aplicar daño
            ThrowerController torre = other.GetComponent<ThrowerController>();
            if (torre != null)
            {
                torre.RecibirImpactoProyectil();
            }
            
            // Efecto visual
            if (efectoImpacto != null)
                Instantiate(efectoImpacto, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Proyectil reciclable tocó el suelo. Destruyendo.");
            
            if (efectoImpacto != null)
                Instantiate(efectoImpacto, transform.position, Quaternion.identity);
                
            Destroy(gameObject);
        }
    }
}