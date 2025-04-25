using UnityEngine;
using UnityEngine.Video;

public class Recoger_cassete : MonoBehaviour
{
    public GameObject cassete;
    public GameObject videoCanvas;
    public VideoClip video;
    public Movimiento movimientoScript;
    public RenderTexture renderDestino;

    void Start()
    {
        videoCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        
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
