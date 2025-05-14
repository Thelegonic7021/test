using System.Collections;
using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 10f;

    [Header("Salto")]
    public float jumpForce = 20f;
    private bool canDoubleJump;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashUpSpeed = 25f;
    public float dashDuration = 0.2f;
    public float verticalImpulse = 5f;
    private bool hasDashed;
    private bool isDashing;
    private float originalGravity;

    [Header("Fast Fall")]
    public float fastFallSpeed = 25f;

    [Header("Wall (Ground) Grab")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isTouchingWall;
    private int wallDir;
    public float wallSlideSpeed = 2f;

    [Header("Slide")]
    public float slideSpeed = 20f;
    public float slideDuration = 0.3f;
    private bool isSliding;

    [Header("Ground Check")]
    public Transform groundCheckpoint;
    private bool isGrounded;

    [Header("Componentes")]
    public Rigidbody2D theRB;

    [Header("Animator")]
    private Animator anim;

    [Header("Ataque")]
    public KeyCode teclaAtaque = KeyCode.X;
    public Transform attackPoint;
    public float attackDuration = 0.5f;
    private bool isAttacking;
    private int lastDir = 1;

    [Header("Parry")]
    public KeyCode parryKey = KeyCode.E;
    public GameObject parryEffect;
    public AudioClip parrySound;
    public Transform parryPoint;
    public float parryDuration = 0.2f;
    public float parryRadius = 1.5f;
    private CircleCollider2D parryCollider;
    private bool isParrying;

    [Header("Interacción")]
    public float radioDeInteraccion = 1.5f;
    public KeyCode teclaInteraccion = KeyCode.C;

    [Header("Inmunidad")]
    public bool esInmune = false;
    public float duracionInmunidad = 2f;
    public SpriteRenderer spriteJugador;
    private Coroutine inmunidadCoroutine = null;

    // Guarda posiciones locales originales de los checks de pared
    private Vector3 leftCheckLocalPos, rightCheckLocalPos;

    void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();
        originalGravity = theRB.gravityScale;

        leftCheckLocalPos = wallCheckLeft ? wallCheckLeft.localPosition : Vector3.zero;
        rightCheckLocalPos = wallCheckRight ? wallCheckRight.localPosition : Vector3.zero;

        if (parryPoint != null)
        {
            parryCollider = parryPoint.GetComponent<CircleCollider2D>();
            if (parryCollider != null)
                parryCollider.enabled = false;
        }

        // Si no asignaste spriteJugador en el inspector, intenta buscarlo
        if (spriteJugador == null)
            spriteJugador = GetComponentInChildren<SpriteRenderer>();

        Debug.Log("[JohnMovement] Inicializado correctamente");
    }

    void Update()
    {
        // Revisa si este componente está activo
        if (!this.enabled)
        {
            Debug.LogWarning("[JohnMovement] Este componente está desactivado!");
            return;
        }

        if (isAttacking || isParrying)
            return;

        float mx = Input.GetAxisRaw("Horizontal");
        bool wantsToJump = Input.GetKeyDown(KeyCode.Space);

        // DEBUG: Verificar input
        if (Mathf.Abs(mx) > 0.1f)
        {
            Debug.Log($"[JohnMovement] Detectado movimiento horizontal: {mx}");
        }

        // Actualiza dirección y escala
        if (Mathf.Abs(mx) > 0.1f)
        {
            lastDir = mx > 0 ? 1 : -1;
            transform.localScale = new Vector3(lastDir, 1, 1);
        }

        // Comprueba si está en suelo
        isGrounded = groundCheckpoint != null && Physics2D.OverlapCircle(groundCheckpoint.position, 0.2f, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = true;
            hasDashed = false;
            if (theRB) theRB.gravityScale = originalGravity;
        }

        // Reposiciona los wallChecks según orientación
        if (wallCheckLeft != null && wallCheckRight != null)
        {
            if (transform.localScale.x < 0)
            {
                wallCheckLeft.localPosition  = new Vector3(Mathf.Abs(leftCheckLocalPos.x),  leftCheckLocalPos.y, 0);
                wallCheckRight.localPosition = new Vector3(-Mathf.Abs(rightCheckLocalPos.x), rightCheckLocalPos.y, 0);
            }
            else
            {
                wallCheckLeft.localPosition  = new Vector3(-Mathf.Abs(leftCheckLocalPos.x),  leftCheckLocalPos.y, 0);
                wallCheckRight.localPosition = new Vector3( Mathf.Abs(rightCheckLocalPos.x), rightCheckLocalPos.y, 0);
            }
        }

        // Detecta contacto con pared
        bool leftTouch = wallCheckLeft != null && Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, groundLayer);
        bool rightTouch = wallCheckRight != null && Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, groundLayer);
        isTouchingWall = leftTouch || rightTouch;
        wallDir = leftTouch ? -1 : 1;

        bool pressingIntoWall = mx == wallDir;
        bool verticalStill = theRB && theRB.linearVelocity.y <= 0f;
        bool wallGrab = isTouchingWall && !isGrounded && pressingIntoWall
                        && verticalStill && !isDashing && !isSliding;

        // Wall slide
        if (wallGrab && theRB)
        {
            theRB.gravityScale = 0f;
            theRB.linearVelocity = new Vector2(0f, -wallSlideSpeed);
        }
        else if (!isDashing && theRB)
        {
            theRB.gravityScale = originalGravity;
        }

        // Movimiento horizontal
        if (!isDashing && !isSliding && !wallGrab && theRB)
        {
            theRB.linearVelocity = new Vector2(mx * moveSpeed, theRB.linearVelocity.y);
            if (Mathf.Abs(mx) > 0.1f)
            {
                Debug.Log($"[JohnMovement] Aplicando velocidad: {mx * moveSpeed}");
            }
        }

        // Salto
        if (wantsToJump && !isSliding && theRB)
        {
            if (isGrounded)
            {
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                Debug.Log("[JohnMovement] Saltando desde el suelo");
            }
            else if (wallGrab)
            {
                theRB.linearVelocity = new Vector2(-wallDir * moveSpeed, jumpForce);
                theRB.gravityScale = originalGravity;
                Debug.Log("[JohnMovement] Saltando desde la pared");
            }
            else if (canDoubleJump)
            {
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                canDoubleJump = false;
                Debug.Log("[JohnMovement] Doble salto");
            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed && !isSliding)
        {
            bool wantsToDashUp = Input.GetKey(KeyCode.W);
            StartCoroutine(Dash(wantsToDashUp));
            Debug.Log("[JohnMovement] Dash activado");
        }

        // Slide
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isSliding && !isDashing)
        {
            StartCoroutine(Slide());
            Debug.Log("[JohnMovement] Slide activado");
        }

        // Ataque
        if (Input.GetKeyDown(teclaAtaque) && !isSliding && !isDashing)
        {
            StartCoroutine(Attack());
            Debug.Log("[JohnMovement] Ataque activado");
        }

        // Parry
        if (Input.GetKeyDown(parryKey))
        {
            StartCoroutine(ActivarParry());
            Debug.Log("[JohnMovement] Parry activado");
        }

        // Interacción
        if (Input.GetKeyDown(teclaInteraccion) && !isAttacking && !isParrying)
        {
            IntentarInteractuar();
            Debug.Log("[JohnMovement] Intentando interactuar");
        }

        // Actualiza Animator
        if (anim != null && theRB != null)
        {
            anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("wallGrab", wallGrab);
        }
    }

    IEnumerator Dash(bool dashUp)
    {
        if (theRB == null) yield break;
        
        hasDashed = true;
        isDashing = true;
        if (anim) anim.SetTrigger("Dash");

        theRB.gravityScale = 0f;
        theRB.linearVelocity = dashUp
            ? new Vector2(lastDir * dashSpeed * 0.5f, dashUpSpeed)
            : new Vector2(lastDir * dashSpeed, verticalImpulse);

        yield return new WaitForSeconds(dashDuration);

        theRB.gravityScale = originalGravity;
        isDashing = false;
    }

    IEnumerator Slide()
    {
        if (theRB == null) yield break;
        
        isSliding = true;
        if (anim) anim.SetTrigger("Slide");

        theRB.linearVelocity = new Vector2(lastDir * slideSpeed, 0f);
        yield return new WaitForSeconds(slideDuration);

        theRB.linearVelocity = Vector2.zero;
        isSliding = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        if (anim) anim.SetTrigger("Atk");

        var hitbox = attackPoint?.GetComponent<CircleCollider2D>();
        if (hitbox != null) hitbox.enabled = true;

        if (attackPoint != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, 1f);
            foreach (var col in hits)
            {
                if (col.CompareTag("Torre"))
                {
                    Destroy(col.gameObject);
                    Debug.Log("¡Torre destruida!");
                }
            }
        }

        yield return new WaitForSeconds(attackDuration);

        if (hitbox != null) hitbox.enabled = false;
        isAttacking = false;
    }

    IEnumerator ActivarParry()
    {
        isParrying = true;
        if (anim) anim.SetTrigger("isparry");

        if (parryEffect != null && parryPoint != null)
            Instantiate(parryEffect, parryPoint.position, Quaternion.identity);
        if (parrySound != null)
            AudioSource.PlayClipAtPoint(parrySound, transform.position);

        if (parryCollider != null)
            parryCollider.enabled = true;

        DevolverProyectilesEnRango();

        yield return new WaitForSeconds(parryDuration);

        if (parryCollider != null)
            parryCollider.enabled = false;
            
        if (anim) anim.ResetTrigger("isparry");
        isParrying = false;
    }

    void DevolverProyectilesEnRango()
    {
        if (parryPoint == null) return;
        
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(parryPoint.position, parryRadius);
        bool proyectilDevuelto = false;
        
        foreach (Collider2D col in hitObjects)
        {
            var proyectil = col.GetComponent<ProyectilReciclable>();
            if (proyectil != null)
            {
                GameObject[] torres = GameObject.FindGameObjectsWithTag("Torre");
                if (torres.Length > 0)
                {
                    GameObject torreObjetivo = null;
                    float distanciaMinima = float.MaxValue;
                    
                    foreach (GameObject torre in torres)
                    {
                        float d = Vector2.Distance(transform.position, torre.transform.position);
                        if (d < distanciaMinima)
                        {
                            distanciaMinima = d;
                            torreObjetivo = torre;
                        }
                    }
                    
                    if (torreObjetivo != null)
                    {
                        Vector2 direccionATorre = (torreObjetivo.transform.position - proyectil.transform.position).normalized;
                        proyectil.Devolver(direccionATorre);
                        proyectilDevuelto = true;
                        Debug.Log("¡Proyectil devuelto hacia la torre!");
                    }
                }
            }
        }
        
        if (!proyectilDevuelto)
            Debug.Log("¡Parry! Pero no hay proyectiles para devolver.");
    }

    void IntentarInteractuar()
{
    // CORRECCIÓN: Physics2D.OverlapCircleAll solo tiene 3 parámetros en este contexto
    Collider2D[] collidersDetectados = Physics2D.OverlapCircleAll(
        transform.position,     // 1. Posición
        radioDeInteraccion,     // 2. Radio de interacción
        Physics2D.AllLayers     // 3. LayerMask: Todas las capas
    );

    ControlAlcantarilla alcantarillaMasCercana = null;
    float distanciaMinima = float.MaxValue;

    foreach (Collider2D col in collidersDetectados)
    {
        if (col.CompareTag("Alcantarilla"))
        {
            Debug.Log($"[Interacción] Alcantarilla detectada: {col.name}");

            var actual = col.GetComponent<ControlAlcantarilla>();
            if (actual != null)
            {
                float d = Vector2.Distance(transform.position, col.transform.position);
                if (d < distanciaMinima)
                {
                    distanciaMinima = d;
                    alcantarillaMasCercana = actual;
                }
            }
        }
    }

    if (alcantarillaMasCercana != null)
    {
        Debug.Log($"[Interacción] Interactuando con: {alcantarillaMasCercana.name}");
        alcantarillaMasCercana.RegistrarInteraccion();
    }
    else
    {
        Debug.Log("No hay alcantarilla cerca para interactuar.");
    }
}
    // MÉTODO CRÍTICO: Inmunidad sin bloquear el movimiento
    public void ActivarInmunidad()
    {
        // Detener corrutina previa si existe
        if (inmunidadCoroutine != null)
        {
            StopCoroutine(inmunidadCoroutine);
            inmunidadCoroutine = null;
        }
        
        // Iniciar nueva inmunidad
        inmunidadCoroutine = StartCoroutine(EfectoInmunidad());
        Debug.Log("[JohnMovement] Inmunidad activada (sin bloqueo de movimiento)");
    }

    private IEnumerator EfectoInmunidad()
    {
        esInmune = true;
        float tiempoInicio = Time.time;
        Debug.Log("[JohnMovement] Iniciando efecto visual de inmunidad, JUGADOR PUEDE MOVERSE");

        // NO desactivamos el componente, solo hacemos el efecto visual
        Color colorOriginal = spriteJugador ? spriteJugador.color : Color.white;
        
        while (Time.time - tiempoInicio < duracionInmunidad)
        {
            if (spriteJugador != null)
                spriteJugador.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0.5f);
            yield return new WaitForSeconds(0.1f);
            if (spriteJugador != null)
                spriteJugador.color = colorOriginal;
            yield return new WaitForSeconds(0.1f);
        }

        // Asegurar que el color vuelve a la normalidad
        if (spriteJugador != null)
            spriteJugador.color = colorOriginal;
            
        Debug.Log("[JohnMovement] Fin de la inmunidad temporal");
        esInmune = false;
        inmunidadCoroutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeInteraccion);

        if (parryPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(parryPoint.position, parryRadius);
        }

        if (groundCheckpoint != null)
            Gizmos.DrawWireSphere(groundCheckpoint.position, 0.2f);
        if (wallCheckLeft != null)
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        if (wallCheckRight != null)
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
    }

    // Para debuggear desde el inspector
    void OnEnable()
    {
        Debug.Log("[JohnMovement] Componente ACTIVADO");
    }

    void OnDisable()
    {
        Debug.Log("[JohnMovement] Componente DESACTIVADO");
    }
}