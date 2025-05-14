using UnityEngine;
using UnityEngine.SceneManagement;
public class Pasar_niveles : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string escena_a_cargar;

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            SceneManager.LoadScene(escena_a_cargar);
        }
    }

}
