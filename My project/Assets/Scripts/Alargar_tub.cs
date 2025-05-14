using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class Alargar_tub : MonoBehaviour
{
    public Activar_ducha activar_DuchaScript;
    private GameObject jugador;
    private bool recogido = false;
    private float offsetY;

    public Movimiento movimientoScript;
    public GameObject agua;
    public Animator animator;
    public bool hayAgua = false;
    public Vector3 posicionEspecifica; 
    public float velocidad = 5f;
    public float distanciaParaRecoger = 1.5f;

    void Awake()
    {
        animator.GetComponent<Animator>();
        agua.SetActive(true);
    }
    void Update()
    {
        if (jugador != null)
        {
            float distancia = Vector2.Distance(transform.position, jugador.transform.position);

            if (Input.GetKeyDown(KeyCode.K ) && !hayAgua)
            {
                if (!recogido && distancia <= distanciaParaRecoger)
                {
                    movimientoScript.Bloqueado = true;
                    animator.SetTrigger("Llenado");
                    StartCoroutine(Ejecucion());
                    
                }
                else if (recogido)
                {
                    if(activar_DuchaScript.dentro == true){
                    transform.position = posicionEspecifica;
                    recogido = false;
                    hayAgua = true;
                    }
                }
            }

            if (recogido)
            {
                Vector3 destino = new Vector3(
                    jugador.transform.position.x,
                    jugador.transform.position.y + offsetY,
                    transform.position.z
                );

                transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * velocidad);
            }
        }
    }

    public IEnumerator Ejecucion(){
        yield return new WaitForSeconds(4f);
        movimientoScript.Bloqueado = false;
        offsetY = transform.position.y - jugador.transform.position.y;
        recogido = true;
        agua.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugador = null;
        }
    }
}