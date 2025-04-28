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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position); // Corregido: 'distanceToPalyer' ➔ 'distanceToPlayer'

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized; // Corregido: 'player' ➔ 'Player'
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

        animator.SetBool("enMovimiento", enMovimiento); // Corregido: 'enMobimiento' y 'enMobimiento' ➔ 'enMovimiento'
    }
}