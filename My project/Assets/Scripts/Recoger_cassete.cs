using UnityEngine;
using UnityEngine.Video;

public class Recoger_cassete : MonoBehaviour
{
    public Movimiento movimientoScript;
    public VideoClip video;

    void OnTriggerEnter2D(Collider2D other)
    {
        movimientoScript.Bloqueado = true;
        VideoPlayer vp = gameObject.AddComponent<VideoPlayer>();
        vp.clip = video;
        vp.Play();
        vp.loopPointReached += VideoTerminado;
    }

    void VideoTerminado(VideoPlayer vp)
    {
        movimientoScript.Bloqueado = false;
        Destroy(this);
    }
}
