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

<<<<<<< HEAD
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoPresentacion.SetActive(true);
            movimientoScript.Bloqueado = true;
            tvAnimator.SetBool("ActivarTv", true);
        }
    }

=======
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            if (!PlayerPrefs.HasKey("NombreJugador")){
                textoPresentacion.SetActive(true);
                movimientoScript.Bloqueado = true;
                tvAnimator.SetBool("ActivarTv", true);
            }else{
                Debug.Log("Nombre ya existe: " + PlayerPrefs.GetString("NombreJugador"));
                // Si ya hay nombre, no mostramos nada
            }
        }
    }


>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
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
