using UnityEngine;

public class Detener_animacion : MonoBehaviour
{
    private Animator animator;
    public Lanzar_Roca lanzar_Roca;
    public void Detener(){
        if (lanzar_Roca.rocaAnimacion.GetCurrentAnimatorStateInfo(0).normalizedTime >=1f){
            lanzar_Roca.rocaAnimacion.speed = 0;
            
        }

    }
}
