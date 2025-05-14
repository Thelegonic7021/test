using UnityEngine;

public class Morir_hitbox: MonoBehaviour
{
    public string escena;
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
            
            perderScript.Morir(escena);
        }
    }
}
