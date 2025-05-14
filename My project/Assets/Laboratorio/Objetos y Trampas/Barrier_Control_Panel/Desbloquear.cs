using UnityEngine;

public class Desbloquear : MonoBehaviour
{
    public Movimiento movimientoScript;
    public GameObject hitbox;
    void desbloquear(){
        movimientoScript.Bloqueado= false;
        Destroy(hitbox);
    }
}
