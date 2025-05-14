using UnityEngine;
using System.Collections;

public class ThrowerController : MonoBehaviour
{
    [Header("Configuración")]
    public float intervaloDisparo = 2f;
    public GameObject proyectilPrefab;
    public GameObject proyectilReciclablePrefab;
    public Transform puntoLanzamiento;
    
    [Header("Patrones de Disparo")]
    public bool dispararNormales = true;
    public bool dispararReciclables = true;
    [Range(0f, 1f)]
    public float probabilidadReciclable = 0.3f; // 30% de probabilidad para proyectiles reciclables

    [Header("Modificadores de Velocidad")]
    public float multiplicadorVelocidadCerrada = 1.2f;
    public float multiplicadorVelocidadRota = 0.8f;

    [Header("Control de Activación")]
    public bool iniciarActivado = false;

    [Header("Debug")]
    public bool puedeDisparar = false;

    [Header("Rango de Disparo")]
    public float distanciaMaximaDisparo = 10f;

    [Header("Sistema de Reciclaje")]
    public int impactosParaDestruir = 3;
    private int impactosRecibidos = 0;

    [Header("Visual")]
    public GameObject efectoDestruccion;

    private Transform jugador;
    private Animator anim;
    private float tiempoUltimoDisparo;
    private float intervaloOriginal;
    private bool inicializado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        intervaloOriginal = intervaloDisparo;
        puedeDisparar = iniciarActivado;
        tiempoUltimoDisparo = Time.time;
        inicializado = true;

        StartCoroutine(BuscarJugadorInfinito());
    }

    private IEnumerator BuscarJugadorInfinito()
    {
        while (true)
        {
            if (jugador == null)
            {
                jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (!inicializado || !puedeDisparar || jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaMaximaDisparo &&
            Time.time - tiempoUltimoDisparo >= intervaloDisparo)
        {
            // Gestionar los disparos según las opciones configuradas
            bool lanzarAlguno = false;
            
            // Dispara un proyectil normal si está habilitado
            if (dispararNormales)
            {
                LanzarProyectilNormal();
                lanzarAlguno = true;
            }
            
            // Decide si lanzar un proyectil reciclable según la probabilidad
            if (dispararReciclables && Random.value <= probabilidadReciclable)
            {
                LanzarProyectilReciclable();
                lanzarAlguno = true;
            }
            
            // Solo actualiza el tiempo si se disparó algo
            if (lanzarAlguno)
            {
                tiempoUltimoDisparo = Time.time;
            }
        }
    }

    public void ActivarDisparos()
    {
        puedeDisparar = true;
        intervaloDisparo = intervaloOriginal;
        tiempoUltimoDisparo = Time.time - intervaloDisparo;
    }

    void LanzarProyectilNormal()
    {
        if (proyectilPrefab == null)
        {
            Debug.LogError($"[{gameObject.name}] ERROR: ¡Prefab del proyectil normal no asignado!", gameObject);
            return;
        }

        LanzarProyectilGenerico(proyectilPrefab, "normal");
    }

    void LanzarProyectilReciclable()
    {
        if (proyectilReciclablePrefab == null)
        {
            Debug.LogError($"[{gameObject.name}] ERROR: ¡Prefab del proyectil reciclable no asignado!", gameObject);
            return;
        }

        LanzarProyectilGenerico(proyectilReciclablePrefab, "reciclable");
    }

    void LanzarProyectilGenerico(GameObject prefab, string tipo)
    {
        Debug.Log($"[{gameObject.name}] Lanzando proyectil {tipo}...");

        if (jugador == null || puntoLanzamiento == null)
        {
            Debug.LogError($"[{gameObject.name}] ERROR: Jugador o punto de lanzamiento no asignado.", gameObject);
            return;
        }

        // Activar animación de lanzamiento
        if (anim != null)
            anim.SetTrigger("lanzar");

        // Calcular dirección hacia el jugador
        Vector2 direccion = (jugador.position - puntoLanzamiento.position).normalized;
        
        // Instanciar el proyectil - SIEMPRE EN EL MISMO PUNTO
        GameObject proyectil = Instantiate(prefab, puntoLanzamiento.position, Quaternion.identity);

        if (proyectil != null)
        {
            // Si es un proyectil reciclable, configuramos su origen
            ProyectilReciclable proyectilRec = proyectil.GetComponent<ProyectilReciclable>();
            if (proyectilRec != null)
            {
                proyectilRec.SetTorreOrigen(gameObject);
            }
            
            // Aplicamos la misma velocidad estándar a todos los proyectiles
            Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direccion * 15f;
            }
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] ERROR: Fallo al instanciar el proyectil {tipo}.", gameObject);
        }
    }

    // Método para registrar un impacto de proyectil devuelto
    public void RecibirImpactoProyectil()
    {
        impactosRecibidos++;
        Debug.Log($"Torre recibió impacto {impactosRecibidos}/{impactosParaDestruir}");
        
        // Efecto visual al recibir impacto (parpadeo o animación)
        StartCoroutine(EfectoImpacto());
        
        // Verifica si debe destruirse
        if (impactosRecibidos >= impactosParaDestruir)
        {
            Debug.Log("¡Torre alcanzó el límite de impactos! Destruyendo...");
            
            // Crear efecto de destrucción
            if (efectoDestruccion != null)
            {
                Instantiate(efectoDestruccion, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }

    // Corrutina para efecto visual de impacto
    private IEnumerator EfectoImpacto()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color colorOriginal = renderer.color;
            renderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.color = colorOriginal;
        }
        else
        {
            yield return null;
        }
    }

    public void NotificarAlcantarillaCerrada()
    {
        intervaloDisparo *= multiplicadorVelocidadCerrada;
    }

    public void NotificarAlcantarillaRota()
    {
        intervaloDisparo *= multiplicadorVelocidadRota;
    }

    void OnDrawGizmosSelected()
    {
        // Mostramos el rango de disparo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaMaximaDisparo);
        
        // Mostrar el punto de lanzamiento
        if (puntoLanzamiento != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(puntoLanzamiento.position, 0.2f);
        }
    }
}