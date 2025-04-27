<<<<<<< HEAD
=======
using Unity.VisualScripting;
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
using UnityEngine;
using UnityEngine.Video;

public class Recoger_cassete : MonoBehaviour
{
    public GameObject cassete;
    public GameObject videoCanvas;
    public VideoClip video;
    public Movimiento movimientoScript;
    public RenderTexture renderDestino;
<<<<<<< HEAD
=======
    

    public static int cantidad = 0;
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2

    void Start()
    {
        videoCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
<<<<<<< HEAD
        
=======
        cantidad++;
        Debug.Log("Hay estos cassetes:" + cantidad);
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
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
<<<<<<< HEAD
=======
    
>>>>>>> a71623de9feef77fc8d50285945b2c13878687b2
}
