using UnityEngine;

public class Detener_Camara : MonoBehaviour
{
    public Camara camaraScript;

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            camaraScript.velocidadCamara = 0f;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            camaraScript.velocidadCamara = 0.5f;
        }
    }
}
