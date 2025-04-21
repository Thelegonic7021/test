using UnityEngine;

public class Morir_hitbox: MonoBehaviour
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
        if (dentro){
            string escena = "Escena_laboratorio";
            perderScript.Morir(escena);
        }
    }
}
