using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;         // Panel que contiene el diálogo
    public TextMeshProUGUI dialogueText;     // Texto de diálogo
    public Animator panelAnimator;           // Animator del panel

    [Header("Typing Effect")]
    public float typingSpeed = 0.04f;        // Velocidad del efecto máquina de escribir

    // Cola de frases
    private Queue<string> sentences;
    // Estado del diálogo
    private bool isDialogueActive = false;
    private string currentSentence;
    private bool isTyping = false;

    // Singleton
    public static DialogueManager instance;

    void Awake()
    {
        // Configuración de Singleton
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sentences = new Queue<string>();
    }

    void Start()
    {
        // Asegura que el panel esté oculto
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive)
        {
            // Detecta Enter / Submit
            if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
            {
                if (isTyping)
                {
                    // Completa la frase instantáneamente
                    StopAllCoroutines();
                    dialogueText.text = currentSentence;
                    isTyping = false;
                }
                else
                {
                    // Siguiente frase
                    DisplayNextSentence();
                }
            }
        }
    }

    /// <summary>
    /// Inicia un nuevo diálogo con las líneas proporcionadas.
    /// </summary>
    public void StartDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning("StartDialogue llamado con diálogo vacío.");
            return;
        }

        isDialogueActive = true;
        // Activa el panel y dispara la animación de entrada
        dialoguePanel.SetActive(true);
        if (panelAnimator != null)
            panelAnimator.SetBool("Show", true);

        sentences.Clear();
        foreach (string line in dialogueLines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    /// <summary>
    /// Muestra la siguiente frase, o termina si ya no quedan.
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    /// <summary>
    /// Corrutina de efecto máquina de escribir.
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    /// <summary>
    /// Finaliza el diálogo, oculta el panel y resetea estados.
    /// </summary>
    public void EndDialogue()
    {
        isDialogueActive = false;
        StopAllCoroutines();

        // Dispara la animación de salida
        if (panelAnimator != null)
            panelAnimator.SetBool("Show", false);
        else
            dialoguePanel.SetActive(false);

        dialogueText.text = "";
        Debug.Log("Fin del diálogo.");
    }

    /// <summary>
    /// Permite consultar si hay un diálogo en curso.
    /// </summary>
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
