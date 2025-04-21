using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class Romper_puerta_perder : MonoBehaviour
{
    public GameObject videoCanvas;
    public Perder perderScript;
    public VideoPlayer videoPlayer;
    public Animator animadorJugador;
    private string escena = "Escena_laboratorio";

    private bool dentro = false;
    private bool yaActivado = false;

    void Start()
    {
        videoCanvas.SetActive(false);
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dentro = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dentro = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && dentro && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(EsperarYReproducirVideo());
        }
    }

    IEnumerator EsperarYReproducirVideo()
    {
        // Iniciar animación
        animadorJugador.SetBool("Attack", true);

        // Iniciar sacudida de cámara
        Camara camaraScript = FindObjectOfType<Camara>();
        if (camaraScript != null)
        {
            camaraScript.EmpezarSacudida();
        }

        // Esperar a que termine la animación
        yield return new WaitUntil(() => !animadorJugador.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        animadorJugador.SetBool("Attack", false);

        // Esperar un segundo más
        yield return new WaitForSeconds(1f);

        // Mostrar el video
        videoCanvas.SetActive(true);
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoCanvas.SetActive(false);
        perderScript.Morir(escena);
    }
}
