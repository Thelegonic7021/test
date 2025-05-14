using UnityEngine;

public class Abrir_capullo : MonoBehaviour
{
    public GameObject flor;
    public GameObject cassete;
    public Animator animator;

    void Start()
    {
        animator = flor.GetComponent<Animator>();
    }
    void Abrir(){
        animator.SetBool("Verdad", true);
        cassete.SetActive(true);
    }
}
