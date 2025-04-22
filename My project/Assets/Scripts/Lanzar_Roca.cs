using UnityEngine;

public class Lanzar_Roca : MonoBehaviour
{
    public Animator rocaAnimacion;
    private bool yaLanzada = false;

    void Awake()
    {
        rocaAnimacion = GetComponent<Animator>();
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !yaLanzada){
            
            rocaAnimacion.SetTrigger("LanzarPiedra");
            yaLanzada = true;
        }
        
        if (yaLanzada && rocaAnimacion.GetCurrentAnimatorStateInfo(0).normalizedTime >=1f){
            rocaAnimacion.speed = 1;
            yaLanzada = false;
        }
    }
}
