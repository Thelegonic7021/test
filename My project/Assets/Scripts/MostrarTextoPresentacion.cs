using UnityEngine;

public class MostrarTextoPresentacion : MonoBehaviour
{

    public GameObject textoPresentacion;

    public Movimiento movimientoScript;
    public Animator tvAnimator;

    void Start()
    {
        textoPresentacion.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoPresentacion.SetActive(true);
            movimientoScript.Bloqueado = true;
            tvAnimator.SetBool("ActivarTv", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoPresentacion.SetActive(false);
            movimientoScript.Bloqueado = false;
            tvAnimator.SetBool("ActivarTv", false);
        }
    }
}
