
using UnityEngine;
using System.Collections.Generic;

public class Secuencia : MonoBehaviour
{   
    public Animator otroAnimator;

    private bool dentro = false;
    private List<KeyCode> secuenciaCorrecta = new List<KeyCode> {
        KeyCode.DownArrow,
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.RightArrow
    };

    private int indiceActual = 0;

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            dentro = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            dentro = false;
        }
    }

    void Update()
    {
        if (dentro == true){
            if (Input.anyKeyDown)
            {
                
                if (Input.GetKeyDown(secuenciaCorrecta[indiceActual])){
                    indiceActual++;
                    if (indiceActual >= secuenciaCorrecta.Count){
                        Debug.Log("¡Secuencia correcta!");
                        otroAnimator.SetBool("comando", true);
                        indiceActual = 0;
                    }
                }
                else
                {
                    
                    Debug.Log("Tecla incorrecta, reiniciando");
                    indiceActual = 0;
                }
            }
        }
    }
}
