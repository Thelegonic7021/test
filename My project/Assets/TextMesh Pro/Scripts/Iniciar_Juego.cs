using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Iniciar_Juego : MonoBehaviour{

    public bool pasar_nivel;
    public int indiceNivel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            indiceNivel +=1;
            Iniciar(indiceNivel);
        }

        if (pasar_nivel){
            Iniciar(indiceNivel);
        }
    }
    public void Iniciar(int indice)
    {
        SceneManager.LoadScene(indice);
    }
}
