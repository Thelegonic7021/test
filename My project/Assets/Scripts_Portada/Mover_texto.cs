using UnityEngine;
using UnityEngine.UI;

public class Mover_texto : MonoBehaviour
{
    public Transform objeto;
    public Vector3 destino;
    public float duracion = 2.0f;
    
    private bool estaMoviendo = false;

    public void Mover()
    {
        if (!estaMoviendo)
            StartCoroutine(MoverSuavemente());
    }

    private System.Collections.IEnumerator MoverSuavemente()
    {
        estaMoviendo = true;
        Vector3 inicio = objeto.position;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            t = Mathf.SmoothStep(0f, 1f, t);
            objeto.position = Vector3.Lerp(inicio, destino, t);
            yield return null;
        }

        objeto.position = destino; 
        estaMoviendo = false;
    }
}
