using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class Recoger_cassete : MonoBehaviour
{
    public GameObject cassete;
    public GameObject videoCanvas;
    public VideoClip video;
    public Movimiento movimientoScript;
    public RenderTexture renderDestino;
    

    public static int cantidad = 0;

    void Start()
    {
        videoCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        cantidad++;
        Debug.Log("Hay estos cassetes:" + cantidad);
        movimientoScript.Bloqueado = true;
        VideoPlayer vp = gameObject.AddComponent<VideoPlayer>();
        vp.renderMode = VideoRenderMode.RenderTexture;
        vp.targetTexture = renderDestino;
        videoCanvas.SetActive(true);
        vp.clip = video;
        vp.Play();
        vp.loopPointReached += VideoTerminado;
    }

    void VideoTerminado(VideoPlayer vp)
    {
        movimientoScript.Bloqueado = false;
        videoCanvas.SetActive(false);
        Destroy(cassete);
        Destroy(this);
    }
    
}
