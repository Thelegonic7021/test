using UnityEngine;
using UnityEngine.SceneManagement;

public class Perder : MonoBehaviour
{
    public string escena;

    public void Morir(string escena){
        
        SceneManager.LoadScene(escena);
<<<<<<< HEAD
        Debug.Log("Cargando escena: " + escena); // Para depurar
=======
        Debug.Log("Cargando escena: " + escena);
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
    }
}
