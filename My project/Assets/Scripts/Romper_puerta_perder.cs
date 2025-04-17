using UnityEngine;

public class Romper_puerta_perder : MonoBehaviour
{
    public Perder perderScript;
    public bool dentro = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            dentro = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            dentro = false;
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.J) && dentro){
            Debug.Log("Entro a collision");
            string escena = "Escena_laboratorio";
            perderScript.Morir(escena);
        }
    }
}
