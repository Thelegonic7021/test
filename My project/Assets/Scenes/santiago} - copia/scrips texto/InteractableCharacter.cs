using UnityEngine;
using System.Collections;

public class InteractableCharacter : MonoBehaviour
{
    [Header("Diálogo")]
    public string[] dialogueLines;
    public KeyCode interactionKey = KeyCode.Return;
    public GameObject interactionPrompt;

    [Header("Comportamiento Post-Diálogo")]
    public bool disappearAfterDialogue = true;
    public float fadeDuration = 1.0f;
    public ThrowerController[] torresParaActivar; // Se pueden asignar varias torres

    private bool playerInRange = false;
    private Coroutine activeDialogueCoroutine = null;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("InteractableCharacter: falta SpriteRenderer en " + gameObject.name);
    }

    void Update()
    {
        if (playerInRange
            && Input.GetKeyDown(interactionKey)
            && DialogueManager.instance != null
            && !DialogueManager.instance.IsDialogueActive()
            && activeDialogueCoroutine == null)
        {
            activeDialogueCoroutine = StartCoroutine(StartDialogueAndHandleAction());
        }
    }

    private IEnumerator StartDialogueAndHandleAction()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        DialogueManager.instance.StartDialogue(dialogueLines);

        while (DialogueManager.instance.IsDialogueActive())
            yield return null;

        Debug.Log("Diálogo terminado para: " + gameObject.name);

        // Activar todas las torres asignadas
        if (torresParaActivar != null && torresParaActivar.Length > 0)
        {
            foreach (ThrowerController torre in torresParaActivar)
            {
                if (torre != null)
                {
                    torre.puedeDisparar = false;
                    torre.ActivarDisparos();
                    Debug.Log($"Torre {torre.gameObject.name} activada");
                }
                else
                {
                    Debug.LogWarning("¡Torre nula en el arreglo!");
                }
            }
        }
        else
        {
            Debug.LogWarning("No hay torres asignadas para activar.");
        }

        // Fade out si se requiere
        if (disappearAfterDialogue && spriteRenderer != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} desactivado tras diálogo.");

        activeDialogueCoroutine = null;
    }

    private IEnumerator FadeOut()
    {
        float alpha = spriteRenderer.color.a;
        Color c = spriteRenderer.color;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            c.a = Mathf.Clamp01(alpha);
            spriteRenderer.color = c;
            yield return null;
        }

        c.a = 0f;
        spriteRenderer.color = c;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null && activeDialogueCoroutine == null)
                interactionPrompt.SetActive(true);
            Debug.Log("Jugador entró en rango de " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
            Debug.Log("Jugador salió del rango de " + gameObject.name);
        }
    }
}
