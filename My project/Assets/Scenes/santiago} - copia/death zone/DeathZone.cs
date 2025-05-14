using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoRespawn = 1.5f;

    [Header("Efectos")]
    public bool usarEfectoParpadeo = true;
    public bool usarEfectoRotacion = true;
    public bool usarEfectoEscala = true;
    public GameObject efectoParticulasPrefab;
    public Color colorMuerte = Color.red;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(ProcesarMuerte(other.gameObject));
    }

    public IEnumerator ProcesarMuerte(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("[DeathZone] Jugador nulo");
            yield break;
        }

        var renderer = player.GetComponentInChildren<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError("[DeathZone] Jugador no tiene SpriteRenderer");
            yield break;
        }

        // IMPORTANTE: Guardamos una referencia al componente de movimiento
        var movimiento = player.GetComponent<JohnMovement>();
        
        // Desactivamos el movimiento solo durante la animación de muerte
        if (movimiento) 
        {
            Debug.Log("[DeathZone] Desactivando movimiento temporalmente");
            movimiento.enabled = false;
        }

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        foreach (var col in player.GetComponents<Collider2D>())
            col.enabled = false;

        // Guardar valores originales
        Color colorOriginal = renderer.color;
        Vector3 escalaOriginal = player.transform.localScale;
        Quaternion rotOriginal = player.transform.rotation;

        // Instanciar partículas
        if (efectoParticulasPrefab)
            Instantiate(efectoParticulasPrefab, player.transform.position, Quaternion.identity);

        // Efectos de muerte
        float duracionEfectos = tiempoRespawn * 0.7f;
        float start = Time.time;
        float end = start + duracionEfectos;

        while (Time.time < end)
        {
            float t = (Time.time - start) / duracionEfectos;

            if (usarEfectoParpadeo)
            {
                float pulse = Mathf.PingPong(t * 15, 1f);
                renderer.color = Color.Lerp(colorOriginal, colorMuerte, pulse);
            }

            if (usarEfectoRotacion)
                player.transform.rotation = rotOriginal * Quaternion.Euler(0, 0, t * 720f);

            if (usarEfectoEscala)
                player.transform.localScale = Vector3.Lerp(escalaOriginal, escalaOriginal * 0.2f, t);

            yield return null;
        }

        // Ocultar y esperar el resto
        renderer.enabled = false;
        yield return new WaitForSeconds(tiempoRespawn - duracionEfectos);

        // Restaurar valores
        renderer.enabled = true;
        renderer.color = colorOriginal;
        player.transform.rotation = rotOriginal;
        player.transform.localScale = escalaOriginal;

        // Finalmente, respawnea
        Respawn(player);
    }

    public void Respawn(GameObject player)
    {
        // Buscar el punto de respawn
        Vector3 pos = RespawnManager.Instance
            ? RespawnManager.Instance.GetSpawnPosition()
            : Vector3.zero;

        Debug.Log($"[DeathZone] Posición de respawn: {pos}");
        
        // Mover jugador a la posición de respawn
        player.transform.position = pos;

        // Restaurar físicas
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;
            rb.linearVelocity = Vector2.zero;
        }

        // Reactivar colliders
        foreach (var col in player.GetComponents<Collider2D>())
            col.enabled = true;

        // CRÍTICO: Reactivar componente de movimiento
        var mov = player.GetComponent<JohnMovement>();
        if (mov != null)
        {
            mov.enabled = true;
            Debug.Log("[DeathZone] Movimiento REACTIVADO");
        }
        else
        {
            Debug.LogError("[DeathZone] Error: Componente JohnMovement no encontrado");
        }

        // Activar inmunidad sin desactivar movimiento
        JohnMovement jugador = player.GetComponent<JohnMovement>();
        if (jugador != null)
        {
            jugador.ActivarInmunidad();
            Debug.Log("[DeathZone] Inmunidad activada, jugador puede moverse");
        }

        // Efecto visual de aparición
        StartCoroutine(EfectoAparicion(player));
    }

    private IEnumerator EfectoAparicion(GameObject player)
    {
        var renderer = player.GetComponentInChildren<SpriteRenderer>();

        if (renderer != null)
        {
            Color orig = renderer.color;
            for (int i = 0; i < 3; i++)
            {
                renderer.color = new Color(orig.r, orig.g, orig.b, 0.3f);
                yield return new WaitForSeconds(0.1f);
                renderer.color = orig;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}