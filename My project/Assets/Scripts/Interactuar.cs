using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;


namespace Tutorial2D
{
    public class Interactuar : MonoBehaviour
    {
    
    public GameObject informacion;
    public GameObject mostrarInformacion;

    public bool informacionHabilitada;
    public bool mostrarInformacionHabilitada;

    public LayerMask personaje;

        void Start()
        {
            informacion.gameObject.SetActive(false);
            mostrarInformacion.gameObject.SetActive(false);
        }

        void Update()
        {
            informacionHabilitada = Physics2D.OverlapCircle(this.transform.position, 1f, personaje);

            if (informacionHabilitada == true){
                informacion.gameObject.SetActive(true);
            }
            if (informacionHabilitada == false){
                informacion.gameObject.SetActive(false);
            }

            mostrarInformacionHabilitada = Physics2D.OverlapCircle(this.transform.position, 1f, personaje);

            if (mostrarInformacionHabilitada == true && Input.GetKeyDown(KeyCode.K)){
                mostrarInformacion.gameObject.SetActive(true);
            }
            if (mostrarInformacionHabilitada == false){
                mostrarInformacion.gameObject.SetActive(false);
            }
        }
    }
}