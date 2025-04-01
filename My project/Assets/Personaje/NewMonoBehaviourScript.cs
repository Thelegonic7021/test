using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;  // Variable para Rigidbody2D

    private bool tocar_suelo = true; // Para detectar si el personaje está en el suelo

    public float Speed = 5f;
    public float JumpForce = 7f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>(); // Ahora correctamente inicializado
    }

    void Update()
    {
        // Movimiento a la derecha
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = false;
            transform.position += new Vector3(0.035f, 0, 0);
        }

        // Movimiento a la izquierda
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = true;
            transform.position += new Vector3(-0.035f, 0, 0);
        }

        // Salto (solo si está en el suelo)
        if (tocar_suelo && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }

        // Agacharse
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _animator.Play("Idle");
            transform.position += new Vector3(0, -0.035f, 0);
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.J))
        {
            _animator.Play("attack");
        }
    }

    // Método de salto
    private void Jump()
    {
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, JumpForce);
        _animator.Play("Jump");
        tocar_suelo = false;
    }

    // Detectar si toca el suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            tocar_suelo = true;
        }
    }

   
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            tocar_suelo = false;
        }
    }
}
