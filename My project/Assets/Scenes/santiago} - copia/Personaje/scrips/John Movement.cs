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

    [Header("Wall (Ground) Grab")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isTouchingWall;
    private int wallDir;               // -1 = izquierda, +1 = derecha
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
    public Transform attackPoint;      
    public float attackDuration = 0.5f;
    private bool isAttacking;
    private int lastDir = 1;           

    // Para reposicionar checks
    private Vector3 leftCheckLocalPos, rightCheckLocalPos;

    void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();
        originalGravity = theRB.gravityScale;

        leftCheckLocalPos  = wallCheckLeft.localPosition;
        rightCheckLocalPos = wallCheckRight.localPosition;
    }

    void Update()
    {
        if (isAttacking) return;

        float mx = Input.GetAxisRaw("Horizontal");
        bool wantsToJump = Input.GetKeyDown(KeyCode.Space);

        // Última dirección + flip
        if (Mathf.Abs(mx) > 0.1f)
        {
            lastDir = mx > 0 ? 1 : -1;
            transform.localScale = new Vector3(lastDir, 1, 1);
        }

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheckpoint.position, 0.2f, groundLayer);
        if (isGrounded)
        {
            canDoubleJump    = true;
            hasDashed        = false;
            theRB.gravityScale = originalGravity;
        }

        // Reposicionar los wallChecks
        if (transform.localScale.x < 0)
        {
            wallCheckLeft.localPosition  = new Vector3( Mathf.Abs(leftCheckLocalPos.x),  leftCheckLocalPos.y, 0);
            wallCheckRight.localPosition = new Vector3(-Mathf.Abs(rightCheckLocalPos.x), rightCheckLocalPos.y, 0);
        }
        else
        {
            wallCheckLeft.localPosition  = new Vector3(-Mathf.Abs(leftCheckLocalPos.x),  leftCheckLocalPos.y, 0);
            wallCheckRight.localPosition = new Vector3( Mathf.Abs(rightCheckLocalPos.x), rightCheckLocalPos.y, 0);
        }

        // Detectar paredes
        bool leftTouch  = Physics2D.OverlapCircle(wallCheckLeft.position,  wallCheckRadius, groundLayer);
        bool rightTouch = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, groundLayer);
        isTouchingWall  = leftTouch || rightTouch;
        wallDir         = leftTouch ? -1 : 1;

        // Wall-grab/slide
        bool pressingIntoWall = mx == wallDir;
        bool verticalStill    = theRB.linearVelocity.y <= 0f;
        bool wallGrab = isTouchingWall && !isGrounded && pressingIntoWall && verticalStill && !isDashing && !isSliding;

        // Si acabamos de saltar cancelamos el wallGrab
        if (wantsToJump)
        {
            wallGrab = false;
            isTouchingWall = false;
        }

        if (wallGrab)
        {
            theRB.gravityScale = 0f;
            theRB.linearVelocity     = new Vector2(0f, -wallSlideSpeed);
        }
        else if (!isDashing)
        {
            theRB.gravityScale = originalGravity;
        }

        // Movimiento lateral
        if (!isDashing && !isSliding && !wallGrab)
            theRB.linearVelocity = new Vector2(mx * moveSpeed, theRB.linearVelocity.y);

        // Salto / doble
        if (wantsToJump && !isSliding)
        {
            if (isGrounded)
                theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
            else if (canDoubleJump)
            {
                theRB.linearVelocity    = new Vector2(theRB.linearVelocity.x, jumpForce);
                canDoubleJump     = false;
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

        // Animator
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("wallGrab", wallGrab);
    }

    IEnumerator Dash()
    {
        hasDashed = true;
        isDashing = true;
        anim.SetTrigger("Dash");

        theRB.gravityScale = 0f;
        theRB.linearVelocity     = new Vector2(lastDir * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        theRB.gravityScale = originalGravity;
        isDashing          = false;
    }

    IEnumerator Slide()
    {
        isSliding = true;
        anim.SetTrigger("Slide");

        theRB.linearVelocity = new Vector2(lastDir * slideSpeed, 0f);
        yield return new WaitForSeconds(slideDuration);

        theRB.linearVelocity = Vector2.zero;
        isSliding      = false;
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
        if (wallCheckLeft != null)
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        if (wallCheckRight != null)
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
    }
}
