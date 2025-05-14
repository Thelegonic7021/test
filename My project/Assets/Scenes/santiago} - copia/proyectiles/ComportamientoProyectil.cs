using UnityEngine;
using UnityEngine.SceneManagement;

public class Proyectil : MonoBehaviour 
{
    [Header("Configuración")]
    public float tiempoDeVidaMaximo = 8.0f;
    public float velocidadInicial = 15.0f;
    
    [Header("Efectos")]
    public GameObject efectoImpacto;
    
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
        // (Aquí podrías aplicar la velocidad inicial con el Rigidbody2D)
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Solo dañar si el jugador no está en modo inmunidad
            JohnMovement jugador = other.GetComponent<JohnMovement>();
            if (jugador != null && !jugador.esInmune)
            {
                Debug.Log("¡Jugador golpeado por proyectil normal! Iniciando respawn...");

                // Usar la nueva API para buscar DeathZone
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
            // Destruir el proyectil siempre que toque al jugador
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Proyectil normal tocó el suelo. Destruyendo.");

            if (efectoImpacto != null)
                Instantiate(efectoImpacto, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
