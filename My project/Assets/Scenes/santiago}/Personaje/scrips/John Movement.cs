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
    public float dashDuration = 0.2f;
    private bool hasDashed;
    private bool isDashing;
    private float originalGravity;

    [Header("Fast Fall")]
    public float fastFallSpeed = 25f;

    [Header("Wall Check")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckRadius = 0.1f;
    public LayerMask wallLayer;
    private bool isTouchingWall;
    private int wallDir; // -1 = izquierda, +1 = derecha
    public float wallSlideSpeed = 2f;

    [Header("Slide")]
    public float slideSpeed = 20f;
    public float slideDuration = 0.3f;
    private bool isSliding;

    [Header("Ground Check")]
    public Transform groundCheckpoint;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Componentes")]
    public Rigidbody2D theRB;

    [Header("Animator")]
    private Animator anim;
    private SpriteRenderer thSr;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackDuration = 0.5f;
    private int lastDir = 1;      // 1 = derecha, -1 = izquierda
    private bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        thSr = GetComponent<SpriteRenderer>();
        theRB = GetComponent<Rigidbody2D>();
        originalGravity = theRB.gravityScale;
    }

    void Update()
    {
        // Si estamos atacando, no procesamos más inputs
        if (isAttacking) return;

        float mx = Input.GetAxisRaw("Horizontal");

        // Actualizar última dirección y voltear con localScale
        if (Mathf.Abs(mx) > 0.1f)
        {
            lastDir = mx > 0 ? 1 : -1;
            transform.localScale = new Vector3(lastDir, 1, 1);
        }

        // Movimiento lateral
        if (!isDashing && !isSliding)
            theRB.linearVelocity = new Vector2(mx * moveSpeed, theRB.linearVelocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheckpoint.position, 0.2f, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = true;
            hasDashed = false;
            theRB.gravityScale = originalGravity;
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && !isSliding)
        {
            if (isGrounded)
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
            else if (canDoubleJump)
            {
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed && !isSliding)
            StartCoroutine(Dash());

        // Slide
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isSliding && !isDashing)
            StartCoroutine(Slide());

        // Ataque
        if (Input.GetKeyDown(KeyCode.C) && !isSliding && !isDashing)
            StartCoroutine(Attack());

        // Parámetros de animación
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    IEnumerator Dash()
    {
        hasDashed = true;
        isDashing = true;
        anim.SetTrigger("Dash");

        float dir = lastDir;
        theRB.linearVelocity = new Vector2(dir * dashSpeed, 0f);
        theRB.gravityScale = 0f;

        yield return new WaitForSeconds(dashDuration);

        theRB.gravityScale = originalGravity;
        isDashing = false;
    }

    IEnumerator Slide()
    {
        isSliding = true;
        anim.SetTrigger("Slide");

        float dir = lastDir;

        theRB.linearVelocity = new Vector2(dir * slideSpeed, 0f);
        yield return new WaitForSeconds(slideDuration);

        theRB.linearVelocity = Vector2.zero;
        isSliding = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        anim.SetTrigger("Atk");

        var hitbox = attackPoint.GetComponent<CircleCollider2D>();
        if (hitbox != null) hitbox.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        if (hitbox != null) hitbox.enabled = false;
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckpoint != null)
            Gizmos.DrawWireSphere(groundCheckpoint.position, 0.2f);
        if (attackPoint != null)
        {
            var hc = attackPoint.GetComponent<CircleCollider2D>();
            if (hc != null)
                Gizmos.DrawWireSphere(attackPoint.position, hc.radius);
        }
    }
}
