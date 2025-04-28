using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeController : MonoBehaviour
{
    public Transform Player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private Animator animator;
    private Vector3 escalaOriginal;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaOriginal = transform.localScale; // Guardamos el tamaño original
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            }
            movement = new Vector2(direction.x, 0);
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        animator.SetBool("enMovimiento", enMovimiento);
    }
        // >>> Aquí agregas el daño al player cuando colisione
        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            scrip playerHealth = collision.gameObject.GetComponent<scrip>();
            if (playerHealth != null)
            {
                playerHealth.TomarDaño(1f); // Solo 1 de vida por toque
            }
        }
    }
}