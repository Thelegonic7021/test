using UnityEngine;

public class Desbloquear_puerta : MonoBehaviour
{
    public GameObject hitboxPuerta;
    public Animator otroAnimator;
    private bool rango = false;

    public void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            rango = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            rango = false;
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.K) && rango){
            otroAnimator.SetBool("abrir", true);
            Destroy(hitboxPuerta.GetComponent<Collider2D>());
        }
    }
}
