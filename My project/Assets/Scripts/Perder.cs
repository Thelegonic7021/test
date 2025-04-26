using UnityEngine;
using UnityEngine.SceneManagement;

public class Perder : MonoBehaviour
{
    public string escena;

    public void Morir(string escena){
        
        SceneManager.LoadScene(escena);
        Debug.Log("Cargando escena: " + escena);
    }
}
