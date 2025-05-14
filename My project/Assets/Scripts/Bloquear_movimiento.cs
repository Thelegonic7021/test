using System;
using UnityEngine;

public class Bloquear_movimiento : MonoBehaviour
{
    public Movimiento movimientoScript;

    
    public void Bloquear(){
        movimientoScript.Bloqueado = true;
        
    }

    public void Desbloquear(){
        movimientoScript.Bloqueado = false;
    }
}
