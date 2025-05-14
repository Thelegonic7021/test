using UnityEngine;

public class MoverUI : MonoBehaviour
{
    public RectTransform uiElemento; 
    public Vector2 destino;           
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
        Vector2 inicio = uiElemento.anchoredPosition;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            t = Mathf.SmoothStep(0f, 1f, t);
            uiElemento.anchoredPosition = Vector2.Lerp(inicio, destino, t);
            yield return null;
        }

        uiElemento.anchoredPosition = destino; 
        estaMoviendo = false;
    }
}
