using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogUI : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI mensajeAdvertencia; // Nuevo: Texto para mostrar advertencias
    public Button yesButton;
    public Button noButton;
    public RecolectorDeItems recolector;

    private bool isDialogVisible = false;
    private bool jugadorCerca = false;
    private bool dialogoYaMostrado = false;

    private void Start()
    {
        if (recolector == null)
        {
            recolector = FindObjectOfType<RecolectorDeItems>();
            if (recolector != null)
            {
                Debug.Log("Recolector automáticamente asignado desde: " + recolector.gameObject.name);
            }
            else
            {
                Debug.LogWarning("Advertencia: No se encontró un RecolectorDeItems en la escena.");
            }
        }

        if (dialogPanel == null)
        {
            Debug.LogError("Falta referencia al Panel de Diálogo");
            return;
        }

        if (questionText == null || yesButton == null || noButton == null)
        {
            Debug.LogError("Faltan elementos asignados al UI del diálogo");
            return;
        }

        if (mensajeAdvertencia != null)
        {
            mensajeAdvertencia.text = "";
        }

        dialogPanel.SetActive(false);
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.K) && !isDialogVisible && !dialogoYaMostrado)
        {
            ShowDialog("¿Me ayudarías con algunos suministros, amigo?");
            dialogoYaMostrado = true;
        }
    }

    public void ShowDialog(string question)
    {
        if (dialogPanel == null || questionText == null) return;

        dialogPanel.SetActive(true);
        questionText.text = question;
        if (mensajeAdvertencia != null) mensajeAdvertencia.text = "";
        isDialogVisible = true;
    }

    private void OnYesClicked()
    {
        if (recolector != null)
        {
            if (recolector.cantidadItems >= 10)
            {
                recolector.QuitarItems(10);
                Debug.Log("Se han quitado 10 ítems.");
                HideDialog();
            }
            else
            {
                Debug.Log("No tienes suficientes ítems.");
                if (mensajeAdvertencia != null)
                {
                    mensajeAdvertencia.text = "No tienes suficientes ítems para ayudar.";
                }
            }
        }
        else
        {
            Debug.LogWarning("Recolector no asignado.");
            HideDialog();
        }
    }

    private void OnNoClicked()
    {
        Debug.Log("Usuario seleccionó NO");
        HideDialog();
    }

    private void HideDialog()
    {
        if (dialogPanel == null) return;

        dialogPanel.SetActive(false);
        isDialogVisible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
            dialogoYaMostrado = false; // Permitir mostrar el diálogo nuevamente al volver
        }
    }
}
