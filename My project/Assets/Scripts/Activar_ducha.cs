using UnityEngine;

public class Activar_ducha : MonoBehaviour
{
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

}
