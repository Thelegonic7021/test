using UnityEngine;

public class Creditos : MonoBehaviour
{
    public GameObject imagen;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
        imagen.SetActive(true);
        }
    }

    

}
