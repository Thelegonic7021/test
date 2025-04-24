using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI;
public class barradevida : MonoBehaviour
{
   public Raw Image rellenoBarraVida;
   private flotar_boton flotar_boton;
   private float vidaMax;
    void Start()
    {
        flotar_boton=GameObject.Find("Player").GetComponent<flotar_boton>();
        vidaMax=flotar_boton.vida;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount=flotar_boton.vida/vidaMax;
    }
}