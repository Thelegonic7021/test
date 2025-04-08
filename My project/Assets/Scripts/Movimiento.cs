using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;  

    private bool tocar_suelo = true; 

    public float Speed = 0.08f;
    public float JumpForce = 7f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        // Movimiento a la derecha
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = false;
            transform.position += new Vector3(Speed, 0, 0);
        }

        // Movimiento a la izquierda
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _animator.Play("Run");
            GetComponent<SpriteRenderer>().flipX = true;
            transform.position += new Vector3(-Speed, 0, 0);
        }

        // Salto (solo si está en el suelo)
        if (tocar_suelo && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
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