using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Iniciar_Juego : MonoBehaviour{

    public string escenaLab = "Escena_laboratorio";
    public void Iniciar()
    {
        SceneManager.LoadScene(escenaLab);
    }
}
