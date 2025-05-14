using UnityEngine;

public class ControlAlcantarilla : MonoBehaviour
{
    // --- ESTADOS POSIBLES DE LA ALCANTARILLA ---
    public enum EstadoAlcantarilla
    {
        Abierta,            // Estado inicial, esperando el primer tap
        EsperandoSegundoTap,// Se dio el primer tap, corriendo el tiempo
        Cerrada,            // Se dieron dos taps a tiempo
        Rota                // Pasó el tiempo o se interactuó cuando no debía
    }

    // --- VARIABLES CONFIGURABLES EN EL INSPECTOR ---
    [Header("Configuración Visual")]
    public Sprite spriteAbiertaCerrada; // Sprite para alcantarilla buena
    public Sprite spriteRota;           // Sprite para alcantarilla rota

    [Header("Configuración Funcional")]
    public float tiempoLimiteSegundoTap = 10f; // Segundos para el segundo tap

    // --- VARIABLES INTERNAS ---
    private EstadoAlcantarilla estadoActual = EstadoAlcantarilla.Abierta;
    private SpriteRenderer miSpriteRenderer;
    private float tiempoDesdePrimerTap = 0f;
    private bool conteoActivo = false;

    // Referencia al lanzador de basura
    private ThrowerController lanzadorEnemigo;

 void Start()
{
    // Obtenemos el SpriteRenderer
    miSpriteRenderer = GetComponent<SpriteRenderer>();
    if (miSpriteRenderer == null)
        Debug.LogError("ControlAlcantarilla necesita un SpriteRenderer en el mismo objeto!");
    
    // CORRECIÓN: Usar FindAnyObjectByType en lugar de FindFirstObjectByType
    lanzadorEnemigo = FindAnyObjectByType<ThrowerController>();
    
    // Si no lo encuentra, intenta buscar con más opciones
    if (lanzadorEnemigo == null)
    {
        // Intenta buscar por tag si lo tuviera
        GameObject lanzadorObj = GameObject.FindWithTag("Torre");
        if (lanzadorObj != null)
            lanzadorEnemigo = lanzadorObj.GetComponent<ThrowerController>();
            
        // Si sigue siendo nulo, busca por nombre
        if (lanzadorEnemigo == null)
        {
            ThrowerController[] todosLosLanzadores = FindObjectsByType<ThrowerController>(FindObjectsSortMode.None);
            if (todosLosLanzadores.Length > 0)
            {
                lanzadorEnemigo = todosLosLanzadores[0];
                Debug.Log($"Se encontró lanzador: {lanzadorEnemigo.name}");
            }
            else
            {
                Debug.LogWarning("No se encontró ningún ThrowerController en la escena. Las notificaciones de alcantarilla no funcionarán.");
            }
        }
    }
    
    // Seteamos el sprite inicial
    ActualizarVisual();
}
    void Update()
    {
        if (!conteoActivo) return;

        tiempoDesdePrimerTap += Time.deltaTime;
        if (tiempoDesdePrimerTap > tiempoLimiteSegundoTap)
        {
            Debug.Log("¡Tiempo agotado para el segundo tap!");
            RomperAlcantarilla();
        }
    }

    public void RegistrarInteraccion()
    {
        if (estadoActual == EstadoAlcantarilla.Abierta)
        {
            // Primer tap
            Debug.Log("Primer tap registrado en " + name);
            estadoActual = EstadoAlcantarilla.EsperandoSegundoTap;
            conteoActivo = true;
            tiempoDesdePrimerTap = 0f;
        }
        else if (estadoActual == EstadoAlcantarilla.EsperandoSegundoTap)
        {
            // Segundo tap a tiempo
            Debug.Log("Segundo tap a tiempo en " + name);
            estadoActual = EstadoAlcantarilla.Cerrada;
            conteoActivo = false;
            tiempoDesdePrimerTap = 0f;
            ActualizarVisual();
            NotificarAlEnemigo(true);
        }
    }

    private void RomperAlcantarilla()
    {
        estadoActual = EstadoAlcantarilla.Rota;
        conteoActivo = false;
        tiempoDesdePrimerTap = 0f;
        ActualizarVisual();
        NotificarAlEnemigo(false);
    }

    private void ActualizarVisual()
    {
        if (miSpriteRenderer == null) return;

        miSpriteRenderer.sprite =
            estadoActual == EstadoAlcantarilla.Rota
            ? spriteRota
            : spriteAbiertaCerrada;
    }

   private void NotificarAlEnemigo(bool cerradaExitosamente)
{
    // Primera comprobación - Intentar buscar lanzador si es nulo
    if (lanzadorEnemigo == null)
    {
        // Intento de última hora para buscar el lanzador
        lanzadorEnemigo = FindAnyObjectByType<ThrowerController>();
        
        // Si sigue siendo nulo, registramos y salimos
        if (lanzadorEnemigo == null)
        {
            string mensaje = cerradaExitosamente 
                ? "¡Alcantarilla cerrada exitosamente! (No hay ThrowerController para notificar)"
                : "Alcantarilla rota. (No hay ThrowerController para notificar)";
                
            Debug.Log(mensaje);
            return;
        }
    }

    try
    {
        // Intenta realizar la notificación con manejo de errores
        if (cerradaExitosamente)
        {
            lanzadorEnemigo.NotificarAlcantarillaCerrada();
            Debug.Log("¡Alcantarilla cerrada exitosamente! ThrowerController notificado.");
        }
        else
        {
            lanzadorEnemigo.NotificarAlcantarillaRota();
            Debug.Log("Alcantarilla rota. ThrowerController notificado.");
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Error al notificar al ThrowerController: {ex.Message}");
    }
}
}