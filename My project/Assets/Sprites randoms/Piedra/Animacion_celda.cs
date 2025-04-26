using UnityEngine;

public class Animacion_celda : MonoBehaviour
{
    public Animator otroAnimator;

    public void Disable_barrer(){
        otroAnimator.SetBool("barrera", true);
    }
}
