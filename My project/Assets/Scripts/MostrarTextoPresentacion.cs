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
